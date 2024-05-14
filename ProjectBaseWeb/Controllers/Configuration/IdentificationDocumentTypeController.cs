using ProjectBaseWeb.Repositories.Configuration;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;

namespace ProjectBaseWeb.Controllers.Configuration
{
    [Route("Configuration/IdentificationDocumentType")]
    public class IdentificationDocumentTypeController
        : BulkSMSGatewayBaseGridController<
            IdentificationDocumentType,
            IdentificationDocumentTypesRepository
        >
    {
        public IdentificationDocumentTypeController()
            : base(new IdentificationDocumentTypesRepository("Identification document type")) { }
    }
}
