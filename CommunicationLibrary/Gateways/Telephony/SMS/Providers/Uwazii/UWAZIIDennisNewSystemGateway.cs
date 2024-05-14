using CommunicationLibrary.Telephony.Messaging.SMS;
using CommunicationLibrary.Telephony.Senders.SMS;
using Newtonsoft.Json;
using ProjectBase.Repository.Transaction;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CommunicationLibrary.Gateways.Telephony.SMS.Providers.Uwazii
{
    public class UWAZIIDennisNewSystemGateway : ISMSGatewayProvider
    {
        public UWAZIIDennisNewSystemGateway()
        {
            ThrowOnException = true;
        }

        public string ProviderName { get { return "UWAZIIDENNISNEWSYSTEMSINGLESMS"; } }
        public bool Supports(string providerName)
        {
            return providerName == ProviderName;
        }

        public ExecutionResult<IEnumerable<TextMessageSendResult>> InitSendSMS(IEnumerable<SMSTextMessage> messageToSend, string userName, string passWord, string senderMaskName)
        {
            this.Clear();

            var sendResult = new ExecutionResult<IEnumerable<TextMessageSendResult>>(true);
            var resultList = new List<TextMessageSendResult>();

            Credential = new NetworkCredential
            {
                UserName = userName,
                Password = passWord,
            };

            foreach (var item in messageToSend)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.Sender))
                        item.Sender = senderMaskName;
                    var resultSend = SendMessage(item);
                    resultList.AddRange(resultSend.Result);
                }
                catch (Exception ex)
                {
                    resultList.Add(new TextMessageSendResult
                    {
                        Destination = item.DestinationAddress.ToString(),
                        Sent = false,
                        StatusText = ex.Message,
                    });
                }
            }
            if (resultList
                .Where(r => r.Sent == true)
                .Any() == false)
            {
                //incase the entire batch fails then report the failure otherwise report the success
                sendResult.IsOkay = false;
                var firstDefault = resultList.FirstOrDefault();
                if (firstDefault != null)
                    sendResult.Message = $"Message sending failed. Error code {firstDefault.ResponseCode}, Reason {firstDefault.StatusText}";
                else
                    sendResult.Message = "Sending SMS failed. Result list was empty";
            }
            sendResult.Result = resultList;
            return sendResult;
        }

        public void Clear()
        {

        }

        public string BaseURI
        {
            get
            {
                return "https://restapi.uwaziimobile.com";
            }
        }

        public string ExceptionMessage { get; private set; }
        public bool ThrowOnException { get; private set; }
        public NetworkCredential Credential { get; private set; }

        public string GetAuthorizationToken(string userName, string password)
        {
            #region authenticate the request
            var client = new RestClient(new Uri(BaseURI));//create the object that will be used to send HTTP requests over the internet from this application 
                                                          //to the server identified by BaseURI

            client.AddDefaultHeader("Content-Type", "application/json");//ensure the application goes with the application/json header for accept
            client.AddDefaultHeader("Type", "json");
            var request = new RestRequest("/v1/authorize", Method.POST);//create an object to send this request just compliments the RestClient

            request.AddJsonBody(new DenisUwaziiAuthorizationRequest
            {
                username = userName,
                password = password
            });

            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            var restClientResult = client.Execute<DenisUwaziiAuthenticateTokenResult>(request);

            if (restClientResult.StatusCode == HttpStatusCode.OK)
                return restClientResult.Data.data.authorization_code;
            else
            {
                ExceptionMessage = string.Format("Message sending failed\nReason gateway server error {1} : HTTPStatusCode {1} ", restClientResult.ErrorMessage, restClientResult.StatusCode);
                throw new InvalidOperationException(ExceptionMessage);
            }

            #endregion
        }

        public string GetAuthenticationAccessToken(string authorization_code)
        {
            #region authenticate the request
            var client = new RestClient(new Uri(BaseURI));//create the object that will be used to send HTTP requests over the internet from this application 
                                                          //to the server identified by BaseURI

            client.AddDefaultHeader("Content-Type", "application/json");//ensure the application goes with the application/json header for accept
            client.AddDefaultHeader("Type", "json");
            var request = new RestRequest("/v1/accesstoken", Method.POST);//create an object to send this request just compliments the RestClient

            request.AddJsonBody(new DenisUwaziiAuthorizationAccessTokenRequest
            {
                authorization_code = authorization_code,
            });

            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            var restClientResult = client.Execute<DenisUwaziiAuthenticateTokenResult>(request);

            if (restClientResult.StatusCode == HttpStatusCode.OK)
                return restClientResult.Data.data.access_token;
            else
            {
                ExceptionMessage = string.Format("Message sending failed\nReason gateway server error {1} : HTTPStatusCode {1} ", restClientResult.ErrorMessage, restClientResult.StatusCode);
                throw new InvalidOperationException(ExceptionMessage);
            }

            #endregion
        }

        public string GetXAccessToken(string userName, string password)
        {
            var authorizationToken = GetAuthorizationToken(userName, password);
            var authenticationToken = GetAuthenticationAccessToken(authorizationToken);
            return authenticationToken;
        }
        public ExecutionResult<IEnumerable<TextMessageSendResult>> SendMessage(SMSTextMessage message)
        {
            //this is the entry position to send SMS message
            #region now send the message
            var client = new RestClient(new Uri(BaseURI));//create the object that will be used to send HTTP requests over the internet from this application 
            //to the server identified by BaseURI

            client.AddDefaultHeader("Content-Type", "application/json");//ensure the application goes with the application/json header for accept
            var access_token = GetXAccessToken(Credential.UserName, Credential.Password);
            client.AddDefaultHeader("X-Access-Token", access_token);
            var request = new RestRequest("/v1/send", Method.POST);//create an object to send this request just compliments the RestClient

            var messageModel = GetGatewayMessageModel(message);
            var listedDeserial = new HashSet<DenisUwaziiSMSSendMessageModel> { messageModel };
            var deserialString = JsonConvert.SerializeObject(listedDeserial);
            request.AddJsonBody(listedDeserial);

            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            var restClientResult = client.Execute(request);

            if (restClientResult.StatusCode == HttpStatusCode.OK)
            {
                ProjectBase.Logs.LogToFileActions.Log(restClientResult.Content, "SMS_SEND");
                //var dataTest = Newtonsoft.Json.JsonConvert.DeserializeObject<DenisUwaziiSMSSendMessageResultModel>(restClientResult.Content,
                //    new Newtonsoft.Json.JsonSerializerSettings
                //    {
                //        PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects,
                //        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                //        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                //    });

                dynamic results = JsonConvert.DeserializeObject(restClientResult.Content);


                if ((object)results == null)
                    throw new ArgumentNullException("results");

                bool status = results.status;
                if (status == false)
                    return new ExecutionResult<IEnumerable<TextMessageSendResult>>(false, "Message sending failed. results.status is false");
                if ((object)results.data == null)
                    return new ExecutionResult<IEnumerable<TextMessageSendResult>>(false, "Message sending failed. results.data is null");


                //Dictionary<string, object> propertyList = ((object)results.data)
                //                     .GetType()
                //                     .GetProperties()
                //                     .ToDictionary(p => p.Name, p => p.GetValue(results.data));

                var individualResults = message.DestinationAddress.Select(rs1 =>
                {
                    bool statusOk = status;
                    string serverUniqueId = rs1.UniqueMessageIdentifier;

                    var returnInnerRes = new TextMessageSendResult
                    {
                        Sent = results.status,
                        Destination = rs1.NormalizedAddress,
                        ClientSentMessageUniqueId = rs1.UniqueMessageIdentifier,//here put the id in sent_sms for this request
                        GateWayUniqueMessageId = serverUniqueId,
                        ResponseCode = "NA",
                        StatusText = "NA",
                    };

                    return returnInnerRes;
                });
                var realResults = new ExecutionResult<IEnumerable<TextMessageSendResult>>
                {
                    IsOkay = individualResults.Any(r => r.Sent == true),//if atleast one message was sent then is okay is true
                    Result = individualResults
                };

                if (realResults.IsOkay
                    && realResults.Result != null)
                {
                    var resultList = realResults.Result.ToList();
                    resultList.ForEach(itemB =>
                    {
                        var itemFound = message.DestinationAddress.FirstOrDefault(r => r.NormalizedAddress == itemB.Destination.Replace("+", ""));
                        if (itemFound != null)
                        {
                            itemB.ClientSentMessageUniqueId = itemFound.UniqueMessageIdentifier;
                        }
                    });
                    realResults.Result = resultList;
                }
                return realResults;
            }
            else
            {
                ExceptionMessage = string.Format("Message sending failed\nReason gateway server error {1} : HTTPStatusCode {1} ", restClientResult.ErrorMessage, restClientResult.StatusCode);
                if (ThrowOnException)
                    throw new InvalidOperationException(ExceptionMessage);
            }
            return new ExecutionResult<IEnumerable<TextMessageSendResult>>(false, ExceptionMessage);
            #endregion
        }
        private ExecutionResult<IEnumerable<TextMessageSendResult>> GetVNBResultModelFromGatewayResult(DenisUwaziiSMSSendMessageResultModel results, DenisUwaziiSMSSendMessageModel request, SMSTextMessage message)
        {
            if (results == null)
                throw new ArgumentNullException("results");


            var individualResults = message.DestinationAddress.Select(rs1 => new TextMessageSendResult
            {
                Sent = results.status,
                Destination = rs1.NormalizedAddress,
                ClientSentMessageUniqueId = rs1.UniqueMessageIdentifier,//here put the id in sent_sms for this request
                GateWayUniqueMessageId = rs1.UniqueMessageIdentifier,
                ResponseCode = "NA",
                StatusText = "NA",
            });
            var finalResult = new ExecutionResult<IEnumerable<TextMessageSendResult>>
            {
                IsOkay = individualResults.Any(r => r.Sent == true),//if atleast one message was sent then is okay is true
                Result = individualResults
            };

            return finalResult;
        }
        private DenisUwaziiSMSSendMessageModel GetGatewayMessageModel(SMSTextMessage messages)
        {
            //incase there is a message in which the sender is not specified use the sender mapped as default sender in web.config
            //if (StringExtensions.IsNullOrEmptyOrWhiteSpaceS(messages.Sender))
            //{
            //    NameValuePairs defaultMask = ConfigNameValuePairSectionReader.GetNameValueConfigSection(ConfigNameValuePairSectionReader.SectionName, "smsGatewayDefaultMaskName");
            //    messages.Sender = defaultMask.Value.ToString();
            //}

            var messageToReturn = new DenisUwaziiSMSSendMessageModel
            {
                senderID = messages.Sender,
                number = messages.DestinationAddress.Select(rp => string.Format("{0}", rp.NormalizedAddress)),
                text = messages.BodyToString(),
                type = "sms",
                delivery = true,
            };

            return messageToReturn;
        }
    }


    public class DenisUwaziiAuthenticateTokenResult
    {
        public bool status { get; set; }
        public DenisUwaziiAuthenticateTokenDataResult data { get; set; }
        //        {
        //    "status": true,
        //    "data": {
        //        "authorization_code": "fac44b7253de1cc5074b930b577b956f",
        //        "expires_at": 1637654553
        //    }
        //}
    }
    public class DenisUwaziiAuthenticateTokenDataResult
    {
        public string authorization_code { get; set; }
        public string access_token { get; set; }
        public int expires_at { get; set; }
    }
    public class DenisUwaziiAuthorizationRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class DenisUwaziiAuthorizationAccessTokenRequest
    {
        public string authorization_code { get; set; }
    }

    public class DenisUwaziiSMSSendMessageModel
    {
        public DenisUwaziiSMSSendMessageModel()
        {
            number = new HashSet<string>();
        }
        public IEnumerable<string> number { get; set; }
        public string senderID { get; set; }
        public string text { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string type { get; set; }
        public bool delivery { get; set; }
    }


    //    {
    //    "status": true,
    //    "data": {
    //        "254717385272": [
    //            {
    //                "id_state": "4c43afb8-4768-46dd-aa51-9e8bca3d3cd9"
    //            }
    //        ],
    //        "254720077438": [
    //            {
    //                "id_state": "ef5f912c-da3f-41d0-ab8d-ef2be1d7f293"
    //            }
    //        ]
    //    }
    //}


    public class DenisUwaziiSMSSendMessageResultModel
    {
        public bool status { get; set; }
        public DenisUwaziiSMSSendMessageResultModel data { get; set; }
    }

    public class DenisUwaziiSMSSendMessageResultDataModel
    {

    }
}