using System.Threading.Tasks;

using ProjectBaseWeb.Controllers.Repository;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using LukurareBackend.Repositories.Accounts;
using ProjectBase.KendoUiSupport;
using LukurareBackend.Models.DashBoard;
using ProjectBase.Repository.Transaction;

namespace Lukurare.Controllers.Accounts
{
    [Route("Accounts/AccountProfiles")]
    public class AccountProfilesController
        : BulkSMSGatewayBaseGridController<AccountEntity, AccountProfileRepository>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AccountProfilesController(IWebHostEnvironment hostingEnvironment)
            : base(new AccountProfileRepository("Account Profiles"))
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("GetBalance"), HttpGet]
        public async Task<BalanceModel> GetBalance()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetBalance();
            return result;
        }

        [Route("GetParentDetails"), HttpGet]
        public async Task<AccountEntity> GetParentDetails()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetParentDetails();
            return result;
        }

        [Route("GetFloatList"), HttpPost]
        public virtual async Task<IActionResult> GetFloatList(
            [FromBody] NeedDataSourceEventArgs args
        )
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetFloatList(args);
            return GetActionResult(result);
        }

        [Route("UploadAccounts"), HttpPost]
        public async Task<IActionResult> UploadAccounts()
        {
            Repository.SetCurrentPrincipal(User);
            string path_for_Uploaded_Files = _hostingEnvironment.WebRootPath;
            var uploaded_files = Request.Form.Files;
            var result = await Repository.UploadAccounts(uploaded_files, path_for_Uploaded_Files);
            return GetActionResult(result);
        }
    }
}
