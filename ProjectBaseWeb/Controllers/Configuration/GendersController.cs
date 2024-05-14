using ProjectBaseWeb.Repositories.Configuration;
using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBaseWeb.Controllers.Configuration
{
    [Route("Configuration/Genders")]
    public class GendersController : BulkSMSGatewayBaseGridController<Gender, GenderRepository>
    {
        public GendersController()
            : base(new GenderRepository("Gender")) { }
    }
}
