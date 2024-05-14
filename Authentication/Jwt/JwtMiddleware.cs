using Authentication.Jwt.Config;
using Authentication.Jwt.Service;
using EFDatabaseModel.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Jwt
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppJwtSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppJwtSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserJwtService userService)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()
                ?.Split(" ")
                .Last();

            if (token != null)
                await attachUserToContext(context, userService, token);

            await _next(context);
        }

        private async Task attachUserToContext(
            HttpContext context,
            IUserJwtService userService,
            string token
        )
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var principal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken
                );

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);

                //attach user to context on successful jwt validation
                //var userObject = userService.GetById(userId);
                //context.Items["User"] = userObject;

                var validUserResult = await userService.GetById(userId);
                if (validUserResult.IsOkay == true)
                {
                    context.Items["User"] = validUserResult.Result;
                    context.User = new UserPrinciple(principal.Identity);
                }
                //else do nothing as below in catch
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
