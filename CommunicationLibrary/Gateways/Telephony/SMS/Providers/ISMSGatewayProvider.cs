using CommunicationLibrary.Telephony.Messaging.SMS;
using CommunicationLibrary.Telephony.Senders.SMS;
using ProjectBase.Repository.Transaction;
using System.Collections.Generic;

namespace CommunicationLibrary.Gateways.Telephony.SMS.Providers
{
    public interface ISMSGatewayProvider
    {
        string ProviderName { get; }

        ExecutionResult<IEnumerable<TextMessageSendResult>> InitSendSMS(
            IEnumerable<SMSTextMessage> messageToSend,
            string userName,
            string passWord,
            string senderMaskName
        );

        bool Supports(string providerName);

        void Clear();
    }
}
