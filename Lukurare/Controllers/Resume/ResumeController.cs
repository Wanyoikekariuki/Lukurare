using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;
using SeekersHubBackend.Models.Resume;
using SeekersHubBackend.Repositories.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeekersHub.Controllers.Resume
{
    
    [Route("Resume/Resume")]
    public class ResumeController : BulkSMSGatewayBaseGridController<UserResume, ResumeRepository>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ResumeController(IWebHostEnvironment hostingEnvironment)
            : base(new ResumeRepository("Resume"))
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("EducationalBackground"), HttpPost]
        public async Task<IActionResult> EducationBackground([FromBody] UserResume.Educational_Background model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.EducationalBackground(model);
            return GetActionResult(result);
        }

        [Route("UpdateEducation"), HttpPut]
        public async Task<IActionResult> UpdateEducation([FromBody] UserResume.Educational_Background model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateEducationalBackground(model);
            return GetActionResult(result);
        }

        [Route("UpdateSkills"), HttpPut]
        public async Task<IActionResult> UpdateSkills([FromBody] UserResume.Skills model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateSkills(model);
            return GetActionResult(result);
        }

        [Route("UpdateExtracurricular"), HttpPut]
        public async Task<IActionResult> UpdateExtracurricular([FromBody] UserResume.Extracurricular model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateExtracurricular(model);
            return GetActionResult(result);
        }

        [Route("UpdateCareer"), HttpPut]
        public async Task<IActionResult> UpdateCareer([FromBody] UserResume.CareerInterest model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateCareer(model);
            return GetActionResult(result);
        }

        [Route("UpdateAvailability"), HttpPut]
        public async Task<IActionResult> UpdateAvailability([FromBody] UserResume.Availability model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateAvailability(model);
            return GetActionResult(result);
        }

        [Route("UpdateCertifications"), HttpPut]
        public async Task<IActionResult> UpdateCertifications([FromBody] UserResume.Certifications model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateCertification(model);
            return GetActionResult(result);
        }

        [Route("UpdateWork"), HttpPut]
        public async Task<IActionResult> UpdateWork([FromBody] UserResume.WorkExperience model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateWork(model);
            return GetActionResult(result);
        }

        [Route("UpdateProject"), HttpPut]
        public async Task<IActionResult> UpdateProject([FromBody] UserResume.ProjectsDone model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateProject(model);
            return GetActionResult(result);
        }

        [Route("UpdateReferences"), HttpPut]
        public async Task<IActionResult> UpdateReferences([FromBody] UserResume.References model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateReferences(model);
            return GetActionResult(result);
        }

        [Route("UpdatePortfolio"), HttpPut]
        public async Task<IActionResult> UpdatePortfolio([FromBody] UserResume.Portfolio model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdatePortfolio(model);
            return GetActionResult(result);
        }

        [Route("Skills"), HttpPost]
        public async Task<IActionResult> Skills([FromBody] UserResume.Skills model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Skills(model);
            return GetActionResult(result);
        }

        [Route("WorkExperience"), HttpPost]
        public async Task<IActionResult> WorkExperience([FromBody] UserResume.WorkExperience model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.WorkExperience(model);
            return GetActionResult(result);
        }
        [Route("Projects"), HttpPost]
        public async Task<IActionResult> Projects([FromBody] UserResume.ProjectsDone model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Projects(model);
            return GetActionResult(result);
        }
        [Route("Extracurricular"), HttpPost]
        public async Task<IActionResult> Extracurricular([FromBody] UserResume.Extracurricular model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Extracurricular(model);
            return GetActionResult(result);
        }
        [Route("CertificationsTraining"), HttpPost]
        public async Task<IActionResult> CertificationsTraining([FromBody] UserResume.Certifications model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CertificationsTraining(model);
            return GetActionResult(result);
        }

        [Route("CareerInterest"), HttpPost]
        public async Task<IActionResult> CareerInterest([FromBody] UserResume.CareerInterest model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CareerInterest(model);
            return GetActionResult(result);
        }

        [Route("References"), HttpPost]
        public async Task<IActionResult> References([FromBody] UserResume.References model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.References(model);
            return GetActionResult(result);
        }


        [Route("Portfolio"), HttpPost]
        public async Task<IActionResult> Portfolio([FromBody] UserResume.Portfolio model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Portfolio(model);
            return GetActionResult(result);
        }

        [Route("Availability"), HttpPost]
        public async Task<IActionResult> Availability([FromBody] UserResume.Availability model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Availability(model);
            return GetActionResult(result);
        }

        [Route("Preferences"), HttpPost]
        public async Task<IActionResult> Preferences([FromBody] UserResume.Preferences model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Preferences(model);
            return GetActionResult(result);
        }

     
        [Route("CheckEducationDetails"), HttpGet]
        public async Task<IActionResult> CheckEducationalDetails()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckEducationalDetails();
            return GetActionResult(result);
        }

        [Route("CheckSkills"), HttpGet]
        public async Task<IActionResult> CheckSkills()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckSkills();
            return GetActionResult(result);
        }
        [Route("CheckWorkExperience"), HttpGet]
        public async Task<IActionResult> CheckWorkExperience()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckWorkExperience();
            return GetActionResult(result);
        }

        [Route("CheckProjects"), HttpGet]
        public async Task<IActionResult> CheckProjects()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckProjects();
            return GetActionResult(result);
        }
        [Route("CheckExtraCurricular"), HttpGet]
        public async Task<IActionResult> CheckExtraCurricular()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckExtraCurricular();
            return GetActionResult(result);
        }
        [Route("CheckCertification"), HttpGet]
        public async Task<IActionResult> CheckCertification()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckCertification();
            return GetActionResult(result);
        }
        [Route("CheckCareer"), HttpGet]
        public async Task<IActionResult> CheckCareer()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckCareer();
            return GetActionResult(result);
        }
        [Route("CheckReference"), HttpGet]
        public async Task<IActionResult> CheckReference()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckReference();
            return GetActionResult(result);
        }

        [Route("CheckPortfolio"), HttpGet]
        public async Task<IActionResult> CheckPortfolio()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckPortfolio();
            return GetActionResult(result);
        }

        [Route("CheckAvailability"), HttpGet]
        public async Task<IActionResult> CheckAvailability()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckAvailability();
            return GetActionResult(result);
        }

        [Route("CheckPreference"), HttpGet]
        public async Task<IActionResult> CheckPreference()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.CheckPreference();
            return GetActionResult(result);
        }
    }
}
