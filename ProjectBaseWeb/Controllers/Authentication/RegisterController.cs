using Authentication.Jwt.Custom;
using Authentication.Jwt.Models;
using Authentication.Jwt.Service;
using Authentication.Repository;

using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBase.Repository.Transaction;
using ProjectBaseWeb.Controllers.Repository;
using System.Threading.Tasks;

namespace ProjectBaseWeb.Controllers.Authentication
{
    [Route("Register")]
    public class RegisterController: ContextModelBaseController<AccountEntity,UserSelfRegistrationRepository,EFDatabaseModelDatabaseContext>
    {
        //private IUserJwtService _userService;

        public RegisterController()//IUserJwtService userService
            : base(new UserSelfRegistrationRepository("Register"))
        {
            //_userService = userService;
        }

        [AllowAnonymous]
        [Route("Register"), HttpPost]
        public virtual async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            Repository.SetCurrentPrincipal(User);

            model.Domain = Request.Host.ToString();
            var result = await Repository.Register(model);

            return GetActionResult(result);
        }

        public void AddDomainToModel(RegisterRequest model)
        {
            if (model == null)
                return;

            model.Domain = CustomAuthenticationHandler.GetDomainDetails(HttpContext.Request);
        }
        [AllowAnonymous]
        [Route("GetEntityType"), HttpGet]
        public async Task<IActionResult> GetEntityType()
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetEntityType();
            return GetActionResult(result);
        }
    }
}
