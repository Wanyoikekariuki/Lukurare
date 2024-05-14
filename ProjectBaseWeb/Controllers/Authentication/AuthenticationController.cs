using Authentication.Jwt.Custom;
using Authentication.Jwt.Models;
using Authentication.Jwt.Service;
using Authentication.Repository;

using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBase.Repository.Transaction;
using ProjectBaseWeb.Controllers.Repository;
using System.Threading.Tasks;

namespace ProjectBaseWeb.Controllers.Authentication
{
    [Route("Authentication")]
    public class AuthenticationController
        : ContextModelBaseController<
            User,
            UserAuthenticationRepository,
            EFDatabaseModelDatabaseContext
        >
    {
        private IUserJwtService _userService;

        public AuthenticationController(IUserJwtService userService)
            : base(new UserAuthenticationRepository("Authentication"))
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [Route("Authenticate"), HttpPost]
        public virtual async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            AddDomainToModel(model);
            var result = await _userService.Authenticate(model);
            SetAuthenticationCookie(result);
            return GetActionResult(result);
        }

        [AllowAnonymous]
        [Route("AuthenticateFromQuery"), HttpPost]
        public virtual async Task<IActionResult> AuthenticateFromQuery(AuthenticateRequest model)
        {
            AddDomainToModel(model);
            var result = await _userService.Authenticate(model);
            SetAuthenticationCookie(result);
            return GetActionResult(result);
        }

        #region renew password section
        [AllowAnonymous]
        [Route("RenewPasswordInitiate"), HttpPost]
        public virtual async Task<IActionResult> RenewPasswordInitiate(
            [FromBody] AuthenticateRequest model
        )
        {
            AddDomainToModel(model);
            var result = await _userService.RenewPasswordInitiate(model);
            SetAuthenticationCookie(result);
            return GetActionResult(result);
        }

        [AllowAnonymous]
        [Route("SendRenewPasswordOTP"), HttpPost]
        public virtual async Task<IActionResult> SendRenewPasswordOTP(
            [FromBody] AuthenticateResponse model
        )
        {
            var result = await _userService.SendRenewPasswordOTP(
                model,
                CustomAuthenticationHandler.GetDomainDetails(HttpContext.Request)
            );
            SetAuthenticationCookie(result);
            return GetActionResult(result);
        }

        [AllowAnonymous]
        [Route("RenewPassword"), HttpPost]
        public virtual async Task<IActionResult> RenewPassword([FromBody] RenewPasswordModel model)
        {
            AddDomainToModel(model);
            var result = await _userService.RenewPassword(model);
            SetAuthenticationCookie(result);
            return GetActionResult(result);
        }

        #endregion


        [Route("EndSession"), HttpGet]
        public virtual async Task<IActionResult> EndSession()
        {
            var result = await _userService.EndSession(
                CustomAuthenticationHandler.GetAuthorizationToken(Request)
            );
            return GetActionResult(result);
        }

        [Route("SessionLoggedIn"), HttpGet]
        public virtual IActionResult SessionLoggedIn()
        {
            Repository.SetCurrentPrincipal(User);
            var userCasted = Repository.GetCurrentPrincipalAsType<UserPrinciple>();
            var result = new ExecutionResult<string>(true, "logged_in", userCasted.User.Username);
            return GetActionResult(result);
        }

        public void AddDomainToModel(AuthenticateRequest model)
        {
            if (model == null)
                return;

            model.Domain = CustomAuthenticationHandler.GetDomainDetails(HttpContext.Request);
            model.RemoteIpAddress = CustomAuthenticationHandler.GetRemoteIpDetails(HttpContext);
        }

        private void SetAuthenticationCookie(
            ProjectBase.Repository.Transaction.ExecutionResult<AuthenticateResponse> result
        )
        {
            if (result.IsOkay)
                CustomAuthenticationHandler.SetResponseCookie(Response, result.Result.Token);
        }

        #region test functions
        //[Authorize]
        [HttpGet]
        [Route("Authed")]
        public IActionResult Authed()
        {
            return Ok("Sawa");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("NoAuth")]
        public IActionResult NoAuth()
        {
            return Ok("Sawa");
        }
        #endregion
    }
}
