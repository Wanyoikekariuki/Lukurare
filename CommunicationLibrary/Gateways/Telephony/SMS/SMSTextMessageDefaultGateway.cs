using CommunicationLibrary.Gateways.Config;
using CommunicationLibrary.Gateways.Telephony.SMS.Providers;
using CommunicationLibrary.Telephony.Messaging.SMS;
using CommunicationLibrary.Telephony.Senders.SMS;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CommunicationLibrary.Gateways.Telephony.SMS
{
    public class SMSTextMessageDefaultGateway
    {
        private readonly SMSGatewayDefination _gatewayDefination;

        public SMSTextMessageDefaultGateway(
            string currentModule,
            SMSGatewayDefination gatewayDefination
        )
        {
            CurrentModule = currentModule;
            TransactionPrefix = "STMG";
            _gatewayDefination = gatewayDefination;
        }

        public string CurrentModule { get; set; }
        public string TransactionPrefix = "STMG";

        void autoAddDefaultCountryCode(IEnumerable<SMSTextMessage> messageToSend)
        {
            try
            {
                //ensure messages are okay
                foreach (var msg in messageToSend)
                {
                    foreach (var msg1 in msg.DestinationAddress)
                    {
                        if (!string.IsNullOrEmpty(msg1.SuppliedAddress))
                        {
                            char elemO = msg1.SuppliedAddress.ElementAt(0);
                            if (elemO == '0')
                                msg1.SuppliedAddress = string.Format(
                                    "254{0}",
                                    msg1.SuppliedAddress.Substring(1)
                                );
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Sends multiple SMS without logging, With Credentials Supplied
        /// </summary>
        /// <param name="messageToSend"></param>
        /// <returns></returns>
        public ExecutionResult<IEnumerable<TextMessageSendResult>> SendSMS(
            IEnumerable<SMSTextMessage> messageToSend,
            string smsGatewayDefaultMaskName,
            string userName,
            string passWordValue,
            string smsGatewayProvider
        )
        {
            autoAddDefaultCountryCode(messageToSend);

            var provider = SMSGatewayProviders.GetProvider(smsGatewayProvider);

            var resultSend = provider.InitSendSMS(
                messageToSend,
                userName,
                passWordValue,
                smsGatewayDefaultMaskName
            );

            return resultSend;
        }

        public ExecutionResult<IEnumerable<TextMessageSendResult>> SendSMS(
            IEnumerable<SMSTextMessage> messageToSend,
            string smsGatewayDefaultMaskName,
            NetworkCredential networkCredential,
            string smsGatewayProvider
        )
        {
            return SendSMS(
                messageToSend,
                smsGatewayDefaultMaskName,
                networkCredential.UserName,
                networkCredential.Password,
                smsGatewayProvider
            );
        }

        /// <summary>
        /// Sends multiple SMS without logging
        /// </summary>
        /// <param name="messageToSend"></param>
        /// <returns></returns>
        public ExecutionResult<IEnumerable<TextMessageSendResult>> SendSMS(
            IEnumerable<SMSTextMessage> messageToSend
        )
        {
            autoAddDefaultCountryCode(messageToSend);

            var provider = SMSGatewayProviders.GetProvider(_gatewayDefination.ProviderName);

            var resultSend = provider.InitSendSMS(
                messageToSend,
                _gatewayDefination.Credential.UserName,
                _gatewayDefination.Credential.Password,
                _gatewayDefination.DefaultMaskName
            );

            return resultSend;
        }

        public ExecutionResult<IEnumerable<TextMessageSendResult>> SendSingleSMS(
            SMSTextMessage messageToSend
        )
        {
            ExecutionResult<IEnumerable<TextMessageSendResult>> sentSmsResult = new ExecutionResult<
                IEnumerable<TextMessageSendResult>
            >(true);
            // (var entity = new smcapture_mvvmEntities())
            {
                string serverMessageId = string.Empty;
                if (messageToSend.DestinationAddress.Any())
                {
                    serverMessageId = messageToSend.DestinationAddress
                        .Where(rs => string.IsNullOrEmpty(rs.UniqueMessageIdentifier) == false)
                        .Select(rs => rs.UniqueMessageIdentifier)
                        .Aggregate((agg, agg2) => string.Format("{0},{1}", agg, agg2));
                }
                //var smsLog = new sent_sms_logs
                //{
                //    destination = messageToSend.DestinationAddress
                //    .Select(r => r.NormalizedAddress)
                //    .Aggregate((agg, agg2) => string.Format("{0},{1}", agg, agg2)),
                //    sent_sms_id = null,
                //    sms_size = 0,
                //    server_message_id = serverMessageId
                //};

                try
                {
                    //entity.sent_sms_logs.Add(smsLog);

                    sentSmsResult = SendSMS(new HashSet<SMSTextMessage>() { messageToSend }); // this.SendSMS(messageToSend);

                    var uniqueMessage = sentSmsResult.Result
                        .Select(rs => rs.ResponseCode)
                        .Aggregate((agg, agg2) => string.Format("{0},{1}", agg, agg2));

                    var serverMessage = sentSmsResult.Result
                        .Select(rs => rs.GateWayUniqueMessageId)
                        .Aggregate((agg, agg2) => string.Format("{0},{1}", agg, agg2));

                    //smsLog.sentsms_gateway_status = uniqueMessage;
                    //smsLog.server_message_id = serverMessage;
                    //smsLog.delivered = sentSmsResult.Result.Where(r => r.Delivered).Any();
                }
                catch (Exception ex)
                {
                    sentSmsResult.IsOkay = false;
                    sentSmsResult.Message = ex.Message;
                    sentSmsResult.TechnicalMessage = ex.ToString();
                    var failedMessage = ex.Message;

                    if (failedMessage.Length > 100)
                        failedMessage = failedMessage.Substring(0, 99);

                    //smsLog.sentsms_gateway_status = failedMessage;
                    //smsLog.delivered = false;
                }
                //entity.SaveChanges();
            }
            return sentSmsResult;
        }
    }
}
