using Authentication.Jwt.Service;
using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Models.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Authentication.Jwt.Custom
{
    public class CustomAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly IUserJwtService userJwtService;

        public CustomAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserJwtService userJwtService
        )
            : base(options, logger, encoder, clock)
        {
            this.userJwtService = userJwtService;
        }

        public static string GetAuthorizationToken(HttpRequest Request)
        {
            string authTokenToUse;
            //incase we find it in cookies then cool we use that token other wise we look for it in auth header
            if (Request.Cookies.TryGetValue(AuthTokenCookieName, out authTokenToUse) == false)
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                    return string.Empty;

                string authorizationHeader = Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authorizationHeader))
                    return string.Empty;

                if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                    return string.Empty;

                authTokenToUse = authorizationHeader.Substring("bearer".Length).Trim();

                if (string.IsNullOrEmpty(authTokenToUse))
                    return string.Empty;
            }
            return authTokenToUse;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var authTokenToUse = GetAuthorizationToken(Request);
                if (string.IsNullOrEmpty(authTokenToUse))
                    return AuthenticateResult.Fail("Unauthorized");
                return await ValidateToken(authTokenToUse);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        private async Task<AuthenticateResult> ValidateToken(string token)
        {
            var validatedTokenResult = await userJwtService.ValidateToken(
                token,
                GetDomainDetails(Context.Request)
            );
            if (validatedTokenResult.IsOkay == false)
                return AuthenticateResult.Fail("Unauthorized");

            //var claims = new List<Claim>
            //{
            //    new Claim("id", validatedTokenResult.Result.Id.ToString()),
            //};
            //var identity = new ClaimsIdentity(claims, Scheme.Name);
            var claims = new List<Claim> { new Claim("id", validatedTokenResult.Result.Id.ToString()) };


            var identity = new ClaimsIdentity(claims, Scheme.Name);

            var newIdentity = identity.Clone();
            var context = new EFDatabaseModelDatabaseContext();
            var userRoles = (await context.Users
                .Join(context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                .Join(context.SystemRoles, x => x.ur.SystemRoleId, sr => sr.Id, (x, sr) => new { x, sr })
                .Where(r => r.x.ur.UserId == validatedTokenResult.Result.Id)


                .ToListAsync()).Select(r => new SystemRole
                {
                    Id = r.sr.Id,
                    RoleName = r.sr.RoleName
                });
            //if (user == null)
            //{
            //    return principal;
            //}

            // Add role claims to cloned identity
            foreach (var role in userRoles)
            {
                var claim = new Claim(newIdentity.RoleClaimType, role.RoleName);
                newIdentity.AddClaim(claim);
            }

            var userLastLoginDataResult = await userJwtService.LastLoginData(
                validatedTokenResult.Result,
                token
            );
            if (userLastLoginDataResult.IsOkay == false)
                return AuthenticateResult.Fail("Unauthorized");
            var lastTime = userLastLoginDataResult.Result.AttemptDate.Date;
            lastTime = lastTime.Add(userLastLoginDataResult.Result.AttemptTime);
            var userLastLoginData = userLastLoginDataResult.Result;
            //var principal = new UserPrinciple(
            //    identity,
            //    validatedTokenResult.Result,
            //    lastTime,
            //    userLastLoginData.IpAddress
            //);
            var principal = new UserPrinciple(newIdentity, validatedTokenResult.Result,
              lastTime, userLastLoginData.IpAddress);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        /// <summary>
        /// Name to Sr security through obscurity
        /// </summary>
        public static string AuthTokenCookieName = "srs";

        public static void SetResponseCookie(HttpResponse response, string token)
        {
            response.Cookies.Append(
                AuthTokenCookieName,
                token,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict }
            );
        }

        public static string GetDomainDetails(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var domainDetails = $"{request.Scheme}://{request.Host}";
            return domainDetails;
        }

        public static string GetRemoteIpDetails(Microsoft.AspNetCore.Http.HttpContext context)
        {
            //https://stackoverflow.com/questions/28664686/how-do-i-get-client-ip-address-in-asp-net-core
            var request = context.Request;
            var ipAddress = request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(ipAddress))
                ipAddress = ipAddress.Split(',')[0];

            if (string.IsNullOrWhiteSpace(ipAddress))
                ipAddress = Convert.ToString(request.HttpContext.Connection.RemoteIpAddress);

            if (string.IsNullOrWhiteSpace(ipAddress))
                ipAddress = request.Headers["REMOTE_ADDR"].FirstOrDefault();

            return ipAddress;
        }
    }
}
