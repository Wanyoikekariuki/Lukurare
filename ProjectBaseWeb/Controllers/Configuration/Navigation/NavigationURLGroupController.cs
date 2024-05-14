using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;
using ProjectBaseWeb.Repositories.Configuration.Navigation;

namespace ProjectBaseWeb.Controllers.Configuration.Navigation
{
    [Route("Configuration/Navigation/NavigationURLGroup")]
    public class NavigationURLGroupController
        : BulkSMSGatewayBaseGridController<NavigationUrlGroup, NavigationURLGroupRepository>
    {
        public NavigationURLGroupController()
            : base(new NavigationURLGroupRepository("Navigation URL Group")) { }
    }
}
