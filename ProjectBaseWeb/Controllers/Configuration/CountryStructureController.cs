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
    [Route("Configuration/CountryStructure")]
    public class CountryStructureController
        : BulkSMSGatewayBaseGridController<CountryStructure, CountryStructureRepository>
    {
        public CountryStructureController()
            : base(new CountryStructureRepository("Country Structures")) { }

        [Route("GetCountryStructureLevel/{levelAdding}"), HttpPost]
        public virtual async Task<IActionResult> GetCountryStructureLevel(
            [FromBody] NeedDataSourceEventArgs args,
            int levelAdding
        )
        {
            var result = await Repository.GetCountryStructureLevel(args, levelAdding);
            return GetActionResult(result);
        }
    }
}
