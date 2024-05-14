using System.Net;

namespace CommunicationLibrary.Gateways.Config
{
    public class SMSGatewayDefination
    {
        public NetworkCredential Credential { get; set; }
        public string ProviderName { get; set; }
        public string DefaultMaskName { get; set; }
    }
}
