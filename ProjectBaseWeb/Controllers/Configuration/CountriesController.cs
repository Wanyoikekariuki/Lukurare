using ProjectBaseWeb.Repositories.Configuration;
using ProjectBaseWeb.Controllers.Repository;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace ProjectBaseWeb.Controllers.Configuration
{
    [Route("Configuration/Countries")]
    public class CountriesController : BulkSMSGatewayBaseGridController<Country, CountryRepository>
    {
        public CountriesController()
            : base(new CountryRepository("Countries")) { }
    }
}
