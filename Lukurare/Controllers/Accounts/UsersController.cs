using System.Threading.Tasks;

using ProjectBaseWeb.Controllers.Repository;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using LukurareBackend.Repositories.Accounts;
using Microsoft.AspNetCore.Authorization;

namespace Lukurare.Controllers.Accounts
{
    [Route("Accounts/Users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : BulkSMSGatewayBaseGridController<User, PatnerUserRepository>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsersController(IWebHostEnvironment hostingEnvironment)
            : base(new PatnerUserRepository("Users"))
        {
            _hostingEnvironment = hostingEnvironment;
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
