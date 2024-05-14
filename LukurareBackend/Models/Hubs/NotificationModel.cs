using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models.Hubs
{
    public class NotificationModel
    {
        public string SenderNo { get; set; }
        public string ReceiverNo { get; set; }
        public string ReceiverEmail { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }        
        //public string IsRead { get; set; }
    }
}
