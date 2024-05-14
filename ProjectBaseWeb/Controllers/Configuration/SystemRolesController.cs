using ProjectBaseWeb.Repositories.Configuration;
using ProjectBaseWeb.Controllers.Repository;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace ProjectBaseWeb.Controllers.Configuration
{
    [Route("Configuration/SystemRoles")]
    public class SystemRolesController
        : BulkSMSGatewayBaseGridController<SystemRole, SystemRoleRepository>
    {
        public SystemRolesController()
            : base(new SystemRoleRepository("SystemRoles")) { }
    }
}
