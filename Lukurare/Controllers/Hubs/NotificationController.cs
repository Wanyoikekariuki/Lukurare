using EFDatabaseModel.DbModel;
using EmigrationBackend.Models;
using EmigrationBackend.Models.Hubs;
using EmigrationBackend.Repositories.Accounts;
using EmigrationBackend.Repositories.Hub;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;
using System.Threading.Tasks;

namespace EmigrationWeb.Controllers.Accounts
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
