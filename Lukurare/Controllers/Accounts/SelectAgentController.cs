using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using LukurareBackend.Models;
using LukurareBackend.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjectBase.KendoUiSupport;
using ProjectBaseWeb.Controllers.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lukurare.Controllers.Accounts
{

    [Route("Accounts/SelectAgent")]
    public class SelectAgentController
        : BulkSMSGatewayBaseGridController<AccountEntity, SelectAgentRepository>
    {

        private readonly IWebHostEnvironment _hostingEnvironment;


        public SelectAgentController(IWebHostEnvironment hostingEnvironment)
            : base(new SelectAgentRepository("SelectAgent"))
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("SelectAgent"), HttpGet]
        public async Task<IActionResult> SelectAgent()
        {
            Repository.SetCurrentPrincipal(User);
            var chooseAgent = await Repository.SelectAgent();
            return GetActionResult(chooseAgent);
        }

        [Route("AgentDetails"), HttpGet]
        public async Task<IActionResult> AgentDetails()
        {
            Repository.SetCurrentPrincipal(User);
            var agentDetails = await Repository.AgentDetails();
            return GetActionResult(agentDetails);
        }

        [Route("CustomerInfo"), HttpGet]
        public async Task<IActionResult> CustomerInfo()
        {
            Repository.SetCurrentPrincipal(User);
            var agentDetails = await Repository.CustomerInfo();
            return GetActionResult(agentDetails);
        }
        [Route("SubmitAgent"), HttpPost]
        public async Task<IActionResult> SubmitAgent([FromBody] AccountEntity model )
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.SubmitAgent(model);
            return GetActionResult(result);
        }
        [Route("UploadedDocuments"), HttpGet]
        public async Task<IActionResult> UploadedDocuments(int id)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.UploadedDocuments(id);
            return GetActionResult(result);
        }
        [Route("ConfirmDocument"), HttpPost]
        public async Task<IActionResult> ConfirmDocument([FromBody] AccountEntitySecondaryIdentificationDocument model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.ConfirmDocument(model);
            return GetActionResult(result);
        }
        [Route("AdditionalDetails"), HttpGet]
        public async Task<IActionResult> AdditionalDetails(int id)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.AdditionalDetails(id);
            return GetActionResult(result);
        }
        [Route("GetAccountType"), HttpGet]
        public async Task<IActionResult> GetAccountType()
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.GetAccountType();
            return GetActionResult(result);
        }
        [Route("BulkAdd"), HttpPost]
        public async Task<IActionResult> BulkAdd([FromBody] AccountEntity model)
        {
            Repository.SetCurrentPrincipal(this.User);
            var result = await Repository.BulkAdd(model);
            return GetActionResult(result);
        }

        [Route("SelectCountry"), HttpGet]
        public async Task<IActionResult> SelectCountry()
        {
            Repository.SetCurrentPrincipal(User);
            var chooseCountry = await Repository.SelectCountry();
            return GetActionResult(chooseCountry);
        }


        [Route("GetPayments"), HttpPost]
        public  async Task<IActionResult> GetPayments(
            [FromBody] NeedDataSourceEventArgs args
        )        
        {
            Repository.SetCurrentPrincipal(User);            
            var result = await Repository.GetPayments(args);
            return GetActionResult(result);
        }
        //[Route("GetBalance"), HttpPost]
        //public async Task<IActionResult> GetBalance(
        //    [FromBody] MfsEntityAccount model
        //)
        //{
        //    Repository.SetCurrentPrincipal(User);
        //    var result = await Repository.GetBalance(model);//
        //    return GetActionResult(result);
        //}

        //[Route("SendClaim"), HttpPost]
        //public async Task<IActionResult> SendClaim([FromBody] AccountEntity entity)
        //{
        //    Repository.SetCurrentPrincipal(User);
        //    var result = await Repository.SendClaim(entity);
        //    return GetActionResult(result);
        //}

        //[Route("SendReversal"), HttpPost]
        //public async Task<IActionResult> SendReversal([FromBody] AccountEntity entity)
        //{
        //    Repository.SetCurrentPrincipal(User);
        //    var result = await Repository.SendReversal(entity);
        //    return GetActionResult(result);
        //}

        [Route("GetAgents"), HttpPost]
        public async Task<IActionResult> GetAgents(
           [FromBody] NeedDataSourceEventArgs args
       )
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetAgents(args);
            return GetActionResult(result);
        }

        [Route("GetAllCustomers"), HttpPost]
        public async Task<IActionResult> GetAllCustomers(
          [FromBody] NeedDataSourceEventArgs args
      )
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetAllCustomers(args);
            return GetActionResult(result);
        }

        [Route("DeclineDocument"), HttpDelete]
        public async Task<IActionResult> DeclineDocument([FromBody] AccountEntitySecondaryIdentificationDocument model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.DeclineDocument(model);
            return GetActionResult(result);
        }

        //    [Route("GetAgentCustomers"), HttpPost]
        //    public async Task<IActionResult> GetAgentCustomers(
        //    [FromBody] AccountEntity
        //    {
        //        Repository.SetCurrentPrincipal(User);
        //        var result = await Repository.GetAgentCustomers();
        //        return GetActionResult(result);
        //    }

        [Route("ConfirmPayments"), HttpPut]
        public async Task<IActionResult> ConfirmPayments([FromBody] MfsEntityAccountTransaction model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.ConfirmPayments(model);
            return GetActionResult(result);
        }

        [Route("RejectPayments"), HttpPut]
        public async Task<IActionResult> RejectPayments([FromBody] MfsEntityAccountTransaction model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.RejectPayments(model);
            return GetActionResult(result);
        }

        //[Route("SendClaimCommission"), HttpPost]
        //public async Task<IActionResult> SendClaimCommission([FromBody] Balances model)
        //{
        //    Repository.SetCurrentPrincipal(User);
        //    var result = await Repository.SendClaimCommission(model);
        //    return GetActionResult(result);
        //}

        //[Route("GetCustomerBalance"), HttpPost]
        //public async Task<IActionResult> SendClaimCommission([FromBody] MfsEntityAccount model)
        //{
        //    Repository.SetCurrentPrincipal(User);
        //    var result = await Repository.GetCustomerBalance(model);
        //    return GetActionResult(result);
        //}

        //[Route("GetBalance"), HttpGet]
        //public async Task<IActionResult> GetBalance()
        //{
        //    Repository.SetCurrentPrincipal(User);
        //    var result = await Repository.GetBalance();
        //    return GetActionResult(result);
        //}
    }
}
