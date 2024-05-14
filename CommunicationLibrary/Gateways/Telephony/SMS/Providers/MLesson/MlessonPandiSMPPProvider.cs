using CommunicationLibrary.Gateways.Telephony.SMS.Providers.Uwazii.Models;
using CommunicationLibrary.Telephony.Messaging.SMS;
using CommunicationLibrary.Telephony.Senders.SMS;
using ProjectBase.Repository.Transaction;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;

namespace CommunicationLibrary.Gateways.Telephony.SMS.Providers.MLesson
{
    /// <summary>
    /// Sending will involve sing to local DB only, All messages are assumed success
    /// </summary>
    public class MlessonPandiSMPPProvider : ISMSGatewayProvider
    {
        public MlessonPandiSMPPProvider() { }

        public string ProviderName
        {
            get { return "MlessonPandiSMPPProvider"; }
        }

        public bool Supports(string providerName)
        {
            return providerName == ProviderName;
        }

        public DateTime? GetScheduleTime(DateTime? dateTime)
        {
            if (dateTime == null)
                return DateTime.UtcNow;
            return dateTime;
        }

        public ExecutionResult<IEnumerable<TextMessageSendResult>> InitSendSMS(
            IEnumerable<SMSTextMessage> messageToSend,
            string userName,
            string passWord,
            string senderMaskName
        )
        {
            Clear();

            var sendResult = new ExecutionResult<IEnumerable<TextMessageSendResult>>(true);
            var resultList = new List<TextMessageSendResult>();
            try
            {
                //logic to send message maximum batch size is 100 messages
                if (
                    messageToSend == null || (messageToSend != null && messageToSend.Any() == false)
                )
                {
                    sendResult.Message = "Message to send List is invlaid";
                    sendResult.IsOkay = false;
                    return sendResult;
                }
                //if (messageToSend.Count() > 200)
                //{
                //    sendResult.Message = "Maximum batch size to send is 200";
                //    sendResult.IsOkay = false;
                //    return sendResult;
                //}

                var messagesToSendDb = new List<SchoolMsSmsToSend>();

                var scheduledDateTime = DateTime.UtcNow;

                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    messageToSend
                        .ToList()
                        .ForEach(r =>
                        {
                            r.DestinationAddress
                                .ToList()
                                .ForEach(r2 =>
                                {
                                    messagesToSendDb.Add(
                                        new SchoolMsSmsToSend
                                        {
                                            PhoneNumber = r2.NormalizedAddress,
                                            Message = r.Body,
                                            SenderId = r.Sender,
                                            ScheduledDateTime = GetScheduleTime(
                                                r.MessageScheduleDate
                                            ),
                                            //DeliveredDate = null
                                        }
                                    );
                                });
                        });

                    context.BulkInsert(messagesToSendDb);
                    context.SaveChanges();
                }
                sendResult.Result = resultList; //this provider does not return result after sending
            }
            catch (Exception ex)
            {
                ProjectBase.Logs.LogToFileActions.Log(ex.ToString(), "MlessonPandiSMPPProvider");
                sendResult = sendResult.CopyFromException(ex);
            }
            return sendResult;
        }

        public void Clear() { }
    }
}
