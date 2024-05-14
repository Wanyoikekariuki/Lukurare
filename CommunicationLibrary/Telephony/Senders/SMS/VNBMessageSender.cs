using CommunicationLibrary.Telephony.Address;
using CommunicationLibrary.Telephony.Messaging;
using CommunicationLibrary.Telephony.Messaging.SMS;
using ProjectBase.Repository.Transaction;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CommunicationLibrary.Telephony.Senders.SMS
{
    public class VNBMessageSender
        : MessageSender<SMSTextMessage, PhoneNumber, string, string, TextMessageSendResult>
    {
        public string BaseURI { get; set; }

        //public string AuthenticationResource { get; set; }
        public string SMSResource { get; set; }

        public VNBMessageSender() { }

        public VNBMessageSender(
            IEnumerable<SMSTextMessage> messages,
            string userName,
            string password,
            bool throwOnException = false
        )
            : base(messages, userName, password, throwOnException)
        {
            //equivalent to the default constructor all calls must land here
            //this must be present in the web.config or app.config of the executing application
            //NameValuePairs baseUriCfg = ConfigNameValuePairSectionReader.GetNameValueConfigSection(ConfigNameValuePairSectionReader.SectionName, "smsGatewayBaseURL");
            //NameValuePairs smsGatewayAuthenticateAtBaseURLCfg = ConfigNameValuePairSectionReader.GetNameValueConfigSection(ConfigNameValuePairSectionReader.SectionName, "smsGatewayAuthenticateAtBaseURL");
            //NameValuePairs authenticationResourceCfg = ConfigNameValuePairSectionReader.GetNameValueConfigSection(ConfigNameValuePairSectionReader.SectionName, "smsGatewayAuthenticationResource");
            //NameValuePairs sMSResourceCfg = ConfigNameValuePairSectionReader.GetNameValueConfigSection(ConfigNameValuePairSectionReader.SectionName, "smsGatewaySendmultipleSMSResource");

            //this.BaseURI = baseUriCfg.Value.ToString();
            //this.AuthenticationResource = authenticationResourceCfg.Value.ToString();
            //this.SMSResource = sMSResourceCfg.Value.ToString();
            //this.Authenticated = !Convert.ToBoolean(smsGatewayAuthenticateAtBaseURLCfg.Value);
            //if (Authenticated)
            //    AuthenticationToken = new Parameter("", "", ParameterType.RequestBody);
        }

        public VNBMessageSender(
            SMSTextMessage message,
            string userName,
            string password,
            bool throwOnException = false
        )
            : this(new HashSet<SMSTextMessage> { message }, userName, password, throwOnException)
        { }

        public VNBMessageSender(
            string message,
            string phoneNumber,
            string userName,
            string password,
            string sender = "",
            bool throwOnException = false,
            bool messageCanBeEmpty = false,
            MessageType messageType = MessageType.PlainText,
            MessageDeliveryNotification messageDeliveryNotification =
                MessageDeliveryNotification.DontNotify
        )
            : this(
                new SMSTextMessage(
                    message,
                    sender,
                    phoneNumber,
                    messageCanBeEmpty,
                    messageType,
                    messageDeliveryNotification,
                    throwOnException
                ),
                userName,
                password,
                throwOnException
            ) { }

        public override ExecutionResult<IEnumerable<TextMessageSendResult>> SendMessage()
        {
            var result = new ExecutionResult<IEnumerable<TextMessageSendResult>>(true);
            if (Authenticated == false)
                AuthenticationToken = Authenticate();
            if (Authenticated == false)
                throw new UnauthorizedAccessException(ExceptionMessage);
            #region now send the message
            var client = new RestClient(new Uri(BaseURI));

            client.AddDefaultHeader("Accept", "application/json");
            var request = new RestRequest(SMSResource, Method.POST);
            if (!string.IsNullOrEmpty(AuthenticationToken.Name))
                request.AddHeader(AuthenticationToken.Name, AuthenticationToken.Value.ToString());
            request.AddJsonBody(MessageToSend); //the default mode is to work with a list of messages
            var restClientResult = client.Execute<ExecutionResult<List<TextMessageSendResult>>>(
                request
            );

            string techMsg = string.Empty;
            Exception ex = null;
            if (restClientResult.StatusCode == HttpStatusCode.OK && restClientResult.Data != null)
            {
                var resultAc = restClientResult.Data.CloneDiffrentType<
                    IEnumerable<TextMessageSendResult>
                >();
                if (restClientResult.Data.Result == null)
                    restClientResult.Data.Result = new List<TextMessageSendResult>();
                resultAc.Result = restClientResult.Data.Result.AsEnumerable();
                #region update the authentication token to prevent expiry
                var authHeader = restClientResult.Headers.FirstOrDefault(
                    r => r.Name == "Authorization"
                );
                if (authHeader != null)
                {
                    Authenticated = true;
                    AuthenticationToken = restClientResult.Headers.FirstOrDefault(
                        r => r.Name == "Authorization"
                    );
                }
                #endregion
                return resultAc;
            }
            else
            {
                Authenticated = false;
                ExceptionMessage =
                    "Message sending failed\nReason server returned null result or dint respond with a valid result";

                ex = new InvalidOperationException(restClientResult.Content);
                if (ThrowOnException)
                    throw ex;
            }
            var resultRet = new ExecutionResult<IEnumerable<TextMessageSendResult>>(
                false,
                ExceptionMessage,
                ex
            );
            return resultRet;
            #endregion
        }

        public override void Dispose()
        {
            //shall implement this later incase it
        }

        public override Parameter Authenticate()
        {
            throw new NotImplementedException();
            //if authentication is required the do it else ignore

            //later adopt logic from Authenticator class for generic authentication to this system
            //var client = new RestClient(new Uri(this.BaseURI));
            //{
            //    client.AddDefaultHeader("Accept", "application/json");
            //    var request = new RestRequest(this.AuthenticationResource, Method.POST);

            //    request.AddJsonBody(Credentials);
            //    var result = client.Execute<LoggedInExecutionResult<ExecutionResult>>(request);

            //    if (result.StatusCode == HttpStatusCode.OK)
            //    {
            //        this.Authenticated = true;
            //        this.AuthenticationToken = result.Headers.FirstOrDefault(r => r.Name == "Authorization");
            //        return this.AuthenticationToken;
            //    }
            //    else if (result.Data != null && result.Data.Result != null)
            //    {
            //        this.ExceptionMessage = result.Data.Result.Message;
            //    }
            //    else
            //    {
            //        this.ExceptionMessage = string.Format("Authentication using the supplied credentials failed to the target SMS gateway server at {0}\n{0},{1}", Credentials.UserName, string.Format("{0}/{1}", new Uri(this.BaseURI), this.AuthenticationResource));
            //    }
            //    return new Parameter("", "", ParameterType.RequestBody);
            //}
        }
    }
}
