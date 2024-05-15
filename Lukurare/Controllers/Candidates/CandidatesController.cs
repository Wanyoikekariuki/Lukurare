using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;
using LukurareBackend.Models;
using LukurareBackend.Repositories.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lukurare.Controllers.Candidates
{
    [Route("Candidates/Candidates")]
    
    public class CandidatesController : BulkSMSGatewayBaseGridController<AccountEntity, CandidatesRepository>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;


        public CandidatesController(IWebHostEnvironment hostingEnvironment)
            : base(new CandidatesRepository("Candidates"))
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("GetAllCandidates"), HttpGet]
        public async Task<IActionResult> GetCandidates()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetAllCandidates();
            return GetActionResult(result);
        }

        [Route("GetMainCompanies"), HttpGet]
        public async Task<IActionResult> GetMainCompanies()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetMainCompanies();
            return GetActionResult(result);
        }

        [Route("GetAllCompanies"), HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetAllCompanies();
            return GetActionResult(result);
        }
        [Route("GetCandidatesDetails"), HttpGet]
        public async Task<IActionResult> CandidateDetails(int id)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetCandidatesDetails(id);
            return GetActionResult(result);
        }

        [Route("GetUserDetails"), HttpGet]
        public async Task<IActionResult> UserDetails()
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetUserDetails();
            return GetActionResult(result);
        }

        [Route("GetLoggedCompanyDetails"), HttpGet]
        public async Task<IActionResult> LoggedCompanyDetails()
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetLoggedCompanyDetails();
            return GetActionResult(result);
        }


        [Route("GetCompanyDetails"), HttpGet]
        public async Task<IActionResult> CompanyDetails(int id)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetCompanyDetails(id);
            return GetActionResult(result);
        }

        [Route("MatchCandidate"), HttpPost]
        public async Task<IActionResult> MatchCandidate([FromBody] MatchCandidate model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.MatchCandidate(model);
            return GetActionResult(result);
        }

        [Route("ApplyProject"), HttpPost]
        public async Task<IActionResult> ApplyProject([FromBody] ApplyProject model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.ApplyProject(model);
            return GetActionResult(result);
        }
        [Route("GetApplications"), HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetApplications();
            return GetActionResult(result);
        }
        [Route("GetApplicationDetails"), HttpGet]
        public async Task<IActionResult> ApplicationDetails(int id)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetApplicationDetails(id);
            return GetActionResult(result);
        }
        [Route("AcceptApplication"), HttpPut]
        public async Task<IActionResult> AcceptApplication([FromBody] AcceptApplication model)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.AcceptApplication(model);
            return GetActionResult(result);
        }
        [Route("RejectApplication"), HttpPut]
        public async Task<IActionResult> RejectApplication([FromBody] AcceptApplication model)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.RejectApplication(model);
            return GetActionResult(result);
        }
    }
}
