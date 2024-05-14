using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBase.Repository;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBaseWeb.Controllers.Repository
{
    [Authorize]
    [Route("api/[controller]")]
    public class ContextModelBaseController<TModel, TRepository, TContext> : ProjectBaseController
        where TModel : class, new()
        where TContext : DbContext
        where TRepository : DbContextRepository<TModel, TContext>
    {
        //we use pairs of fromBody and Empty for this link reason
        //https://andrewlock.net/model-binding-json-posts-in-asp-net-core/

        public readonly TRepository Repository;

        public ContextModelBaseController(TRepository repository)
        {
            Repository = repository;
        }

        //[Route("Test")]
        //[HttpGet]
        //public IActionResult Test()
        //{
        //    return Ok("Hello world!");
        //}
    }
}
