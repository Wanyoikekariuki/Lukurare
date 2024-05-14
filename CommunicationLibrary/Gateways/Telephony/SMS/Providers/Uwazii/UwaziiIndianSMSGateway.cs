using CommunicationLibrary.Gateways.Telephony.SMS.Providers.Uwazii.Models;
using CommunicationLibrary.Telephony.Messaging.SMS;
using CommunicationLibrary.Telephony.Senders.SMS;
using ProjectBase.Repository.Transaction;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CommunicationLibrary.Gateways.Telephony.SMS.Providers.Uwazii
{
    public class UwaziiIndianSMSGateway : ISMSGatewayProvider
    {
        public UwaziiIndianSMSGateway()
        {
            ThrowOnException = true;
        }

        public string ProviderName
        {
            get { return "UWAZIIINDIANGATEWAY"; }
        }

        public bool Supports(string providerName)
        {
            return providerName == ProviderName;
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

            Credential = new NetworkCredential { UserName = userName, Password = passWord, };

            foreach (var item in messageToSend)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.Sender))
                        item.Sender = senderMaskName;
                    var resultSend = SendMessage(item);
                    if (resultSend.Result != null)
                        resultList.AddRange(resultSend.Result);
                }
                catch (Exception ex)
                {
                    resultList.Add(
                        new TextMessageSendResult
                        {
                            Destination = item.DestinationAddress.ToString(),
                            Sent = false,
                            StatusText = ex.Message,
                        }
                    );
                }
            }
            if (resultList.Where(r => r.Sent == true).Any() == false)
            {
                //incase the entire batch fails then report the failure otherwise report the success
                sendResult.IsOkay = false;
                var firstDefault = resultList.FirstOrDefault();
                if (firstDefault != null)
                    sendResult.Message =
                        $"Message sending failed. Error code {firstDefault.ResponseCode}, Reason {firstDefault.StatusText}";
                else
                    sendResult.Message = "Sending SMS failed. Result list was empty";
            }
            sendResult.Result = resultList;
            return sendResult;
        }

        public void Clear() { }

        //public string BaseURI
        //{
        //    get
        //    {
        //        return "http://sms.techpayapis.com:6005";
        //    }
        //}

        public string BaseURI
        {
            get { return "http://164.52.197.71:6005"; }
        }

        //http://164.52.197.71/
        public string ExceptionMessage { get; private set; }
        public bool ThrowOnException { get; private set; }
        public NetworkCredential Credential { get; private set; }

        public ExecutionResult<IEnumerable<TextMessageSendResult>> SendMessage(
            SMSTextMessage message
        )
        {
            //this is the entry position to send SMS message
            #region now send the message
            var client = new RestClient(new Uri(BaseURI)); //create the object that will be used to send HTTP requests over the internet from this application
            //to the server identified by BaseURI

            client.AddDefaultHeader("Content-Type", "application/json"); //ensure the application goes with the application/json header for accept
            client.AddDefaultHeader("Type", "json");
            var request = new RestRequest("/api/v2/SendBulkSMS", Method.POST); //create an object to send this request just compliments the RestClient
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Type", "json");

            var messageModel = GetGatewayMessageModel(message);
            #region prevent invalid messages from submission
            Func<UwaziiNewSysMessageParameters, bool> validMessagesFunc = (r) =>
            {
                if (string.IsNullOrEmpty(r.Number))
                    return false; //the number can not be empty
                if (string.IsNullOrWhiteSpace(r.Number))
                    return false; //the number can not be empty
                if (r.Number.StartsWith("254") == false)
                    return false; //number must start with 254
                if (r.Number.Length != "254708238279".Length)
                    return false; //the length of the number supplied should be 12
                //By the time we get here the number is not empty, its 12 character long and it starts with 254
                return true;
            };
            var allMessages = messageModel.MessageParameters.ToList();
            var invalidMessages = messageModel.MessageParameters.Where(
                r => validMessagesFunc(r) == false
            );
            var validMessages = messageModel.MessageParameters.Where(
                r => validMessagesFunc(r) == true
            );
            #endregion

            messageModel.MessageParameters = validMessages.ToList();

            request.AddJsonBody(messageModel);
            IRestResponse<UwaziiNewSysResultMessageModel> restClientResult = null;
            if (messageModel.MessageParameters.Any())
                restClientResult = client.Execute<UwaziiNewSysResultMessageModel>(request);

            if (restClientResult == null && invalidMessages.Count() == allMessages.Count())
            {
                List<TextMessageSendResult> resultList = new List<TextMessageSendResult>();
                foreach (var item in invalidMessages)
                {
                    resultList.Add(
                        new TextMessageSendResult
                        {
                            ClientSentMessageUniqueId = item.UniqueMessageId,
                            Destination = item.Number,
                            GateWayUniqueMessageId = item.Number,
                            Sent = false,
                            ResponseCode = "-1",
                            StatusText = "Invalid number"
                        }
                    );
                }
                return new ExecutionResult<IEnumerable<TextMessageSendResult>>(
                    true,
                    "",
                    resultList
                );
            }
            else if (restClientResult.StatusCode == HttpStatusCode.OK && restClientResult != null)
            {
                var dataTest =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<UwaziiNewSysResultMessageModel>(
                        restClientResult.Content,
                        new Newtonsoft.Json.JsonSerializerSettings
                        {
                            PreserveReferencesHandling = Newtonsoft
                                .Json
                                .PreserveReferencesHandling
                                .Objects,
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                        }
                    );

                var realResults = GetVNBResultModelFromGatewayResult(dataTest);
                if (realResults.Result != null)
                {
                    var resultList = realResults.Result.ToList();
                    resultList.ForEach(itemB =>
                    {
                        var itemFound = message.DestinationAddress.FirstOrDefault(
                            r => r.NormalizedAddress == itemB.Destination.Replace("+", "")
                        );
                        if (itemFound != null)
                            itemB.ClientSentMessageUniqueId = itemFound.UniqueMessageIdentifier;
                    });
                    foreach (var item in invalidMessages)
                    {
                        resultList.Add(
                            new TextMessageSendResult
                            {
                                ClientSentMessageUniqueId = item.UniqueMessageId,
                                Destination = item.Number,
                                GateWayUniqueMessageId = item.Number,
                                Sent = false,
                                ResponseCode = "-1",
                                StatusText = "Invalid number"
                            }
                        );
                    }
                    realResults.Result = resultList;
                }
                else if (realResults.IsOkay == false && dataTest == null)
                {
                    var txtResult = new TextMessageSendResult
                    {
                        ResponseCode = dataTest.ErrorCode,
                        StatusText = dataTest.ErrorDescription,
                    };
                    realResults.Result = new HashSet<TextMessageSendResult> { txtResult };
                    return realResults;
                }
                if (realResults.Result == null)
                {
                    realResults.IsOkay = false;
                    realResults.Message =
                        $"{realResults.Message} .Invalid result list was returned by the server";

                    var txtResult = new TextMessageSendResult
                    {
                        ResponseCode = "-1",
                        StatusText = restClientResult.Content,
                    };
                    realResults.Result = new HashSet<TextMessageSendResult> { txtResult };
                }
                return realResults;
            }
            else
            {
                ExceptionMessage = string.Format(
                    "Message sending failed\nReason gateway server error {1} : HTTPStatusCode {1} ",
                    restClientResult.ErrorMessage,
                    restClientResult.StatusCode
                );
                if (ThrowOnException)
                    throw new InvalidOperationException(ExceptionMessage);
            }
            return new ExecutionResult<IEnumerable<TextMessageSendResult>>(false, ExceptionMessage);
            #endregion
        }

        private ExecutionResult<
            IEnumerable<TextMessageSendResult>
        > GetVNBResultModelFromGatewayResult(UwaziiNewSysResultMessageModel results)
        {
            if (results == null)
                throw new ArgumentNullException("results");

            if (results.Data == null)
                return new ExecutionResult<IEnumerable<TextMessageSendResult>>(
                    false,
                    $"{results.ErrorDescription}, Error Code {results.ErrorCode}"
                );

            var individualResults = results.Data.Select(
                rs1 =>
                    new TextMessageSendResult
                    {
                        Sent = rs1.MessageErrorCode == 0 ? true : false,
                        Destination = rs1.MobileNumber,
                        ClientSentMessageUniqueId = rs1.MessageId, //here put the id in sent_sms for this request
                        GateWayUniqueMessageId = rs1.MessageId,
                        ResponseCode = rs1.MessageErrorCode.ToString(),
                        StatusText = rs1.MessageErrorDescription,
                    }
            );
            var finalResult = new ExecutionResult<IEnumerable<TextMessageSendResult>>();
            finalResult.IsOkay = individualResults.Any(r => r.Sent == true); //if atleast one message was sent then is okay is true
            finalResult.Result = individualResults;

            return finalResult;
        }

        private UwaziiNewSysSendMessageModel GetGatewayMessageModel(SMSTextMessage messages)
        {
            //incase there is a message in which the sender is not specified use the sender mapped as default sender in web.config
            //if (string.IsNullOrEmpty(messages.Sender))
            //{
            //    NameValuePairs defaultMask = ConfigNameValuePairSectionReader.GetNameValueConfigSection(ConfigNameValuePairSectionReader.SectionName, "smsGatewayDefaultMaskName");
            //    messages.Sender = defaultMask.Value.ToString();
            //}

            var messageToReturn = new UwaziiNewSysSendMessageModel
            {
                ClientId = Credential.UserName,
                ApiKey = Credential.Password,
                SenderId = messages.Sender,
                MessageParameters = messages.DestinationAddress.Select(
                    r =>
                        new UwaziiNewSysMessageParameters
                        {
                            Number = r.NormalizedAddress,
                            Text = messages.BodyToString(),
                            ServiceId = r.UniqueMessageIdentifier,
                            UniqueMessageId = r.UniqueMessageIdentifier,
                        }
                ),
                Message = messages.BodyToString(),
            };

            return messageToReturn;
        }
    }
}
