using ProjectBaseWeb.Repositories.Configuration;
using ProjectBaseWeb.Controllers.Repository;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace ProjectBaseWeb.Controllers.Configuration
{
    [Route("Configuration/SupportedUserTypes")]
    public class SupportedUserTypesController
        : BulkSMSGatewayBaseGridController<SupportedUserType, SupportedUserTypeRepository>
    {
        public SupportedUserTypesController()
            : base(new SupportedUserTypeRepository("SupportedUserTypes")) { }
    }
}
