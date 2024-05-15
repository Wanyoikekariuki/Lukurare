using System.Threading.Tasks;
using ProjectBaseWeb.Controllers.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using LukurareBackend.Models.DashBoard;
using LukurareBackend.Repositories.DashBoard;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using System;

namespace Lukurare.Controllers.DashBoard
{
    [Route("DashBoard/Dashboard")]
    public class DashboardController : BulkSMSGatewayBaseGridController<DashBoardModel, DashBoardModelRepository>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DashboardController(IWebHostEnvironment hostingEnvironment)
            : base(new DashBoardModelRepository("Dashboard"))
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("GetDashboardSummary"), HttpGet]
        public async Task<IActionResult> GetDashboardSummary()
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetDashboardSummary();
            return GetActionResult(result);
        }

        [Route("GetCharts"), HttpGet]
        public async Task<IActionResult> GetCharts()
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetCharts();
            return GetActionResult(result);
        }

        [Route("UploadAccounts"), HttpPost]
        public async Task<IActionResult> UploadAccounts()
        {
            Repository.SetCurrentPrincipal(User);
            string path_for_Uploaded_Files = _hostingEnvironment.WebRootPath;
            var uploaded_files = Request.Form.Files;
            var result = await Repository.UploadAccounts(uploaded_files, path_for_Uploaded_Files);
            return GetActionResult(result);
        }

        [Route("CheckRequiredDocuments"), HttpGet]
        public async Task<IActionResult> CheckRequiredDocuments()
        {
            Repository.SetCurrentPrincipal(User);
            var requiredDocuments = await Repository.CheckRequiredDocuments();
            return GetActionResult(requiredDocuments);
        }      

        [Route("SubmitUserUploadDetails"), HttpPost]
        public async Task<IActionResult> SubmitUserUploadDetails([FromBody] KeyNames model)
        {
            Repository.SetCurrentPrincipal(User);
            var requiredDetails = await Repository.SubmitUserUploadDetails(model);
            return GetActionResult(requiredDetails);
        }

        [Route("GetUserType"), HttpGet]
        public async Task<IActionResult> GetUserType()
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetUserType();
            return GetActionResult(result);
        }
        [Route("GetAllMessages"), HttpGet]
        public async Task<IActionResult> GetAllMessages() 
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetAllMessages();
            return GetActionResult(result);
        }


    }
}