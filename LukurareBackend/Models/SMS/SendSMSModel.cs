using System;
using System.Collections.Generic;
using System.Text;
using EFDatabaseModel.DbModel;

namespace LukurareBackend.Models.SMS
{
    public class SendSMSModel
    {
        public SendSMSModel()
        {
            UploadedFileLinks = new HashSet<string>();
        }

        public bool IsReply { get; set; }
        public string CampaignName { get; set; }
        public string PhoneNumbers { get; set; }
        public AccountEntityGroup AccountEntityGroup { get; set; }
        public bool Scheduled { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public IEnumerable<string> UploadedFileLinks { get; set; }
        public string MessageToSend { get; set; }
    }
}
