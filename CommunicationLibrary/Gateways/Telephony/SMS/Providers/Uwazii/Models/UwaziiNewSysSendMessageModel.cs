using System.Collections.Generic;

namespace CommunicationLibrary.Gateways.Telephony.SMS.Providers.Uwazii.Models
{
    public class UwaziiNewSysSendMessageModel
    {
        public string SenderId { get; set; }
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public string Message { get; set; }
        public string MobileNumbers { get; set; }

        public IEnumerable<UwaziiNewSysMessageParameters> MessageParameters { get; set; }

        public UwaziiNewSysSendMessageModel()
        {
            MessageParameters = new HashSet<UwaziiNewSysMessageParameters>();
        }
    }

    public class UwaziiNewSysMessageParameters
    {
        public string Number { get; set; }
        public string Text { get; set; }
        public string ServiceId { get; set; }
        public string UniqueMessageId { get; set; }
    }

    public class UwaziiNewSysResultMessageModel
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public IEnumerable<UwaziiNewSysResultMessageModelData> Data { get; set; }

        public UwaziiNewSysResultMessageModel()
        {
            Data = new HashSet<UwaziiNewSysResultMessageModelData>();
        }
    }

    public class UwaziiNewSysResultMessageModelData
    {
        public string MobileNumber { get; set; }
        public string MessageId { get; set; }
        public int MessageErrorCode { get; set; }
        public string MessageErrorDescription { get; set; }
    }
}
