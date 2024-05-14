using ProjectBaseWeb.Repositories.Configuration;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using ProjectBase.KendoUiSupport;
using ProjectBaseWeb.Controllers.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBaseWeb.Controllers.Configuration
{
    [Route("Configuration/CountryStructureLevels")]
    public class CountryStructureLevelController
        : BulkSMSGatewayBaseGridController<CountryStructureLevel, CountryStructureLevelRepository>
    {
        public CountryStructureLevelController()
            : base(new CountryStructureLevelRepository("Country Structure Levels")) { }
    }
}
