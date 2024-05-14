using Microsoft.AspNetCore.Mvc;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBaseWeb.Controllers.Repository
{
    public class ProjectBaseController : ControllerBase
    {
        protected IActionResult GetActionResult<T>(ITransactionResult<T> result)
        {
            if (result == null)
                return BadRequest();
            else if (result.IsOkay)
                return Ok(result);
            else
                return NotFound(result);
        }
    }
}
