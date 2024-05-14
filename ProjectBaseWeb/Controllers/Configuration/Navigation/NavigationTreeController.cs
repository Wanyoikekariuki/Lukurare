using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using ProjectBase.KendoUiSupport;
using ProjectBaseWeb.Controllers.Repository;
using ProjectBaseWeb.Repositories.Configuration.Navigation;
using System.Threading.Tasks;

namespace ProjectBaseWeb.Controllers.Configuration.Navigation
{
    [Route("Configuration/Navigation/NavigationTree")]
    public class NavigationTreeController
        : BulkSMSGatewayBaseGridController<NavigationUrl, NavigationTreeRepository>
    {
        public NavigationTreeController()
            : base(new NavigationTreeRepository("Navigation Tree")) { }

        [Route("GetNavigationURLGroupData"), HttpPost]
        public virtual async Task<IActionResult> GetNavigationURLGroupData(
            [FromBody] NeedDataSourceEventArgs args
        )
        {
            var result = await Repository.GetNavigationURLGroupData(args);
            return GetActionResult(result);
        }
    }
}
