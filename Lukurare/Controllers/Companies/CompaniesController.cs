using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;
using LukurareBackend.Models.Companies;
using LukurareBackend.Repositories.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lukurare.Controllers.Companies
{
    [Route("Companies/Companies")]   
    public class CompaniesController : BulkSMSGatewayBaseGridController<AccountEntity, CompaniesRepository>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CompaniesController(IWebHostEnvironment hostingEnvironment)
            : base(new CompaniesRepository("Companies"))
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("CompanyOverview"), HttpPost]
        public async Task<IActionResult> CompanyOverview([FromBody] AllCompanies.CompanyOverview model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CompanyOverview(model);
            return GetActionResult(result);
        }

        [Route("UpdateCompanyOverview"), HttpPut]
        public async Task<IActionResult> UpdateCompanyOverview([FromBody] AllCompanies.CompanyOverview model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateCompanyOverview(model);
            return GetActionResult(result);
        }

        [Route("UpdateProjects"), HttpPut]
        public async Task<IActionResult> UpdateProjects([FromBody] AllCompanies.Projects model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateProjects(model);
            return GetActionResult(result);
        }

        [Route("UpdateInternship"), HttpPut]
        public async Task<IActionResult> UpdateInternship([FromBody] AllCompanies.InternshipType model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateInternship(model);
            return GetActionResult(result);
        }

        [Route("UpdateCompetencies"), HttpPut]
        public async Task<IActionResult> UpdateCompetencies([FromBody] AllCompanies.CoreCompetencies model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Competencies(model);
            return GetActionResult(result);
        }

        [Route("UpdateCompanyCulture"), HttpPut]
        public async Task<IActionResult> UpdateCompanyCulture([FromBody] AllCompanies.CompanyCulture model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CompanyCulture(model);
            return GetActionResult(result);
        }

        [Route("UpdateTechnology"), HttpPut]
        public async Task<IActionResult> UpdateTechnology([FromBody] AllCompanies.TechnologicalCapabilities model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateTechnology(model);
            return GetActionResult(result);
        }

        [Route("UpdateSocialResponsibility"), HttpPut]
        public async Task<IActionResult> UpdateSocialResponsibility([FromBody] AllCompanies.SocialResponsibility model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateSocialResponsibility(model);
            return GetActionResult(result);
        }

        [Route("UpdateAwards"), HttpPut]
        public async Task<IActionResult> UpdateAwards([FromBody] AllCompanies.Awards model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateAwards(model);
            return GetActionResult(result);
        }

        [Route("UpdateCareer"), HttpPut]
        public async Task<IActionResult> UpdateCareer([FromBody] AllCompanies.CareerOpportunities model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateCareer(model);
            return GetActionResult(result);
        }

        [Route("UpdatePreferredIntern"), HttpPut]
        public async Task<IActionResult> UpdatePreferredIntern([FromBody] AllCompanies.InternProfile model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdatePreferredIntern(model);
            return GetActionResult(result);
        }

        [Route("Projects"), HttpPost]
        public async Task<IActionResult> Projects([FromBody] AllCompanies.Projects model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Projects(model);
            return GetActionResult(result);
        }

        [Route("Internship"), HttpPost]
        public async Task<IActionResult> Internship([FromBody] AllCompanies.InternshipType model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.InternshipType(model);
            return GetActionResult(result);
        }

        [Route("Competencies"), HttpPost]
        public async Task<IActionResult> Competencies([FromBody] AllCompanies.CoreCompetencies model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Competencies(model);
            return GetActionResult(result);
        }

        [Route("CompanyCulture"), HttpPost]
        public async Task<IActionResult> CompanyCulture([FromBody] AllCompanies.CompanyCulture model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CompanyCulture(model);
            return GetActionResult(result);
        }

        [Route("Technology"), HttpPost]
        public async Task<IActionResult> Technology([FromBody] AllCompanies.TechnologicalCapabilities model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Technology(model);
            return GetActionResult(result);
        }

        [Route("SocialResponsibility"), HttpPost]
        public async Task<IActionResult> SocialResponsibility([FromBody] AllCompanies.SocialResponsibility model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.SocialResponsibility(model);
            return GetActionResult(result);
        }

        [Route("Awards"), HttpPost]
        public async Task<IActionResult> Awards([FromBody] AllCompanies.Awards model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Awards(model);
            return GetActionResult(result);
        }

        [Route("Career"), HttpPost]
        public async Task<IActionResult> Career([FromBody] AllCompanies.CareerOpportunities model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Career(model);
            return GetActionResult(result);
        }

        [Route("PreferredIntern"), HttpPost]
        public async Task<IActionResult> PreferredIntern([FromBody] AllCompanies.InternProfile model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.PreferredIntern(model);
            return GetActionResult(result);
        }

        [Route("CompanyProject"), HttpPost]
        public async Task<IActionResult> CompanyProject([FromBody] CompanyProject model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CompanyProject(model);
            return GetActionResult(result);
        }

        [Route("CheckCompany"), HttpGet]
        public async Task<IActionResult> CheckCompanyOverview()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckCompany();
            return GetActionResult(result);
        }
        [Route("CheckProjects"), HttpGet]
        public async Task<IActionResult> CheckProjects()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckProjects();
            return GetActionResult(result);
        }
        [Route("CheckInternship"), HttpGet]
        public async Task<IActionResult> CheckInternships()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckInternships();
            return GetActionResult(result);
        }
        [Route("CheckCompetence"), HttpGet]
        public async Task<IActionResult> CheckCompetence()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckCompetence();
            return GetActionResult(result);
        }
        [Route("CheckCulture"), HttpGet]
        public async Task<IActionResult> CheckCulture()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckCulture();
            return GetActionResult(result);
        }

        [Route("CheckTechnology"), HttpGet]
        public async Task<IActionResult> CheckTechnology()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckTechnology();
            return GetActionResult(result);
        }
        [Route("CheckSocial"), HttpGet]
        public async Task<IActionResult> CheckSocial()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckSocial();
            return GetActionResult(result);
        }

        [Route("CheckAward"), HttpGet]
        public async Task<IActionResult> CheckAward()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckAward();
            return GetActionResult(result);
        }

        [Route("CheckCareer"), HttpGet]
        public async Task<IActionResult> CheckCareer()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckCareer();
            return GetActionResult(result);
        }

        [Route("CheckIntern"), HttpGet]
        public async Task<IActionResult> CheckIntern()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckIntern();
            return GetActionResult(result);
        }
        [Route("GetCompanyCandidates"), HttpGet]
        public async Task<IActionResult> GetCompanyCandidates()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetCompanyCandidates();
            return GetActionResult(result);
        }
        [Route("GetSpecificProjects"), HttpGet]
        public async Task<IActionResult> GetSpecificProjects()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetSpecificProjects();
            return GetActionResult(result);
        }
        [AllowAnonymous]
        [Route("GetAllProjects"), HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetAllProjects();
            return GetActionResult(result);
        }
        [AllowAnonymous]
        [Route("GetProjectDetails"), HttpGet]
        public async Task<IActionResult> ProjectDetails(int id)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetProjectDetails(id);
            return GetActionResult(result);
        }
        [AllowAnonymous]
        [Route("SearchProjects"), HttpGet]
        public async Task<IActionResult> SearchProjects(string industry, string location)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.SearchProjects(industry,location);
            return GetActionResult(result);
        }
    }
}
