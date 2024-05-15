using EFDatabaseModel.DbModel;
using LukurareBackend.Models;
using LukurareBackend.Models.Hubs;
using LukurareBackend.Repositories.Accounts;
using LukurareBackend.Repositories.Hub;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;
using System.Threading.Tasks;

namespace Lukurare.Controllers.Accounts
{

    [Route("Hubs/Notification")]
    public class NotificationController
        : BulkSMSGatewayBaseGridController<NotificationModel, ChatRepository>
    {

        private readonly IWebHostEnvironment _hostingEnvironment;


        public NotificationController(IWebHostEnvironment hostingEnvironment)
            : base(new ChatRepository("Notification"))
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("Send"), HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] NotificationModel model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.SendNotification(model);
            return GetActionResult(result);
        }
        [Route("GetNotifications"), HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetNotifications();
            return GetActionResult(result);
        }

        [Route("UpdateNotifications"), HttpPost]
        public async Task<IActionResult> UpdateNotifications([FromBody] SchoolMsSmsToSend model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.UpdateNotification(model);
            return GetActionResult(result);
        }

        //Chat 
        [Route("GetContactList"), HttpGet]
        public async Task<IActionResult> GetContactList() 
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetContactList();
            return GetActionResult(result);
        }

    }
}
