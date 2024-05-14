using ProjectBaseWeb.Repositories.Configuration;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;

namespace ProjectBaseWeb.Controllers.Configuration
{
    [Route("Configuration/SystemModules")]
    public class SystemModulesController
        : BulkSMSGatewayBaseGridController<SystemModule, SystemModuleRepository>
    {
        public SystemModulesController()
            : base(new SystemModuleRepository("System Modules")) { }
    }
}
