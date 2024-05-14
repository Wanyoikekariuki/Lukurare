using System.Threading.Tasks;
using ProjectBaseWeb.Controllers.Repository;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using ProjectBase.KendoUiSupport;
using ProjectBaseWeb.Repositories.Configuration;

namespace ProjectBaseWeb.Controllers.Configuration
{
    [Route("Configuration/CompanyModules")]
    public class CompanyModulesController
        : BulkSMSGatewayBaseGridController<CompanyModule, CompanyModuleRepository>
    {
        public CompanyModulesController()
            : base(new CompanyModuleRepository("Company Modules")) { }

        [Route("GetSubAccount"), HttpPost]
        public virtual async Task<IActionResult> GetSubAccount(
            [FromBody] NeedDataSourceEventArgs args
        )
        {
            var result = await Repository.GetSubAccount(args);
            return GetActionResult(result);
        }
    }
}
