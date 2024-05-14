using ProjectBaseWeb.Repositories.Configuration;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;

namespace ProjectBaseWeb.Controllers.Configuration
{
    [Route("Configuration/TransactionTypes")]
    public class TransactionTypesController
        : BulkSMSGatewayBaseGridController<TransactionType, TransactionTypeRepository>
    {
        public TransactionTypesController()
            : base(new TransactionTypeRepository("TransactionTypes")) { }
    }
}
