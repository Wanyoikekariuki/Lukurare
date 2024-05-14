using CommunicationLibrary.Gateways.Telephony.SMS.Providers.MLesson;
using CommunicationLibrary.Gateways.Telephony.SMS.Providers.Uwazii;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunicationLibrary.Gateways.Telephony.SMS.Providers
{
    public static class SMSGatewayProviders
    {
        public static List<ISMSGatewayProvider> ISMSGatewayProviders { get; set; }

        public static bool SMSGatewayProvidersInited = false;

        public static void InitSMSGatewayProviders()
        {
            if (SMSGatewayProvidersInited == true)
                return;
            if (SMSGatewayProvidersInited == false)
                SMSGatewayProvidersInited = true;

            ISMSGatewayProviders = new List<ISMSGatewayProvider>();
            //ISMSGatewayProviders.Add(new VIAUwaziGateway());
            //ISMSGatewayProviders.Add(new AfricasTalkingMessageSender());
            //ISMSGatewayProviders.Add(new UWAZIINewSystemGateway());
            ISMSGatewayProviders.Add(new UwaziiIndianSMSGateway());
            ISMSGatewayProviders.Add(new MlessonPandiSMPPProvider());
        }

        public static ISMSGatewayProvider GetProvider(string providerName)
        {
            InitSMSGatewayProviders();
            var provider = ISMSGatewayProviders.FirstOrDefault(r => r.ProviderName == providerName);
            if (provider == null)
                throw new ArgumentException(
                    string.Format("Sorry the bulk sms provider {0} is not configured", providerName)
                );
            return provider;
        }
    }
}
