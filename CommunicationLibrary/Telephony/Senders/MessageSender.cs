using CommunicationLibrary.Address;
using CommunicationLibrary.Telephony.Messaging;
using ProjectBase.Repository.Transaction;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CommunicationLibrary.Telephony.Senders
{
    public abstract class MessageSender<
        SentMessageType,
        AddrressType,
        AddressValueType,
        SentMessageBodyType,
        SentMessageResultType
    >
        : IMessageSender<
            SentMessageType,
            AddrressType,
            AddressValueType,
            SentMessageBodyType,
            SentMessageResultType
        >,
            IDisposable
        where SentMessageBodyType : class
        where AddrressType : class, ICommunicationAddress<AddressValueType>
        where SentMessageResultType : class
        where AddressValueType : class
        where SentMessageType : IMessage<SentMessageBodyType, AddrressType, AddressValueType>
    {
        protected MessageSender() { }

        private MessageSender(string userName, string password)
            : this()
        {
            Credentials = new NetworkCredential(userName, password);
        }

        protected MessageSender(
            IEnumerable<SentMessageType> message,
            string userName,
            string password,
            bool throwOnException = false
        )
            : this(userName, password)
        {
            MessageToSend = message;
            ThrowOnException = throwOnException;
        }

        //protected MessageSender(SentMessageType message, string userName, string password, bool throwOnException = false)
        //    : this(new HashSet<SentMessageType> { message }, userName, password, throwOnException)
        //{

        //}

        /// <summary>
        /// The actual message to send
        /// </summary>
        public IEnumerable<SentMessageType> MessageToSend { get; set; }

        /// <summary>
        /// The method that does the actuall sending of the messages
        /// </summary>
        /// <returns>A custom execution result that contains Sent Boolen or IsOkay to show if the message was send</returns>
        public abstract ExecutionResult<IEnumerable<SentMessageResultType>> SendMessage();

        ///// <summary>
        ///// The method that does the actuall sending of the messages
        ///// </summary>
        ///// <returns>A custom execution result that contains Sent Boolen or IsOkay to show if the message was send</returns>
        //public abstract IMessageSendResult<SentMessageResultType, ClientSentMessageUniqueIdType, GateWaySentMessageUniqueIdType> SendSingleMessage();

        /// <summary>
        /// Get or set the exception message associated with the current IMessage
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets a boolean values that indicates wether or not we should throw on exception
        /// </summary>
        public bool IsValid()
        {
            bool isvalid = true;

            //ensure that the message is valid//on message ensure adress is valid
            if (MessageToSend.Where(r => r.IsValid() == false).Any())
            {
                ExceptionMessage = string.Format(
                    "Invalid Message, Error Message {0}",
                    MessageToSend.FirstOrDefault(r => r.IsValid() == false).ExceptionMessage
                );
                isvalid = false;
            }
            else if (MessageToSend.Any() == false)
            {
                ExceptionMessage = string.Format(
                    "Invalid Message, Error Message :The submited messages should contain atleast one message\nMessage list is empty"
                );
                isvalid = false;
            }
            if (isvalid == false && ThrowOnException == true)
                throw new InvalidOperationException(ExceptionMessage);
            return isvalid;
        }

        /// <summary>
        /// To ensure a specified message is valid or within acceptable valid limits of a certain type of message
        /// </summary>
        /// <returns>Boolean indicating wether the above is true</returns>
        public bool ThrowOnException { get; set; }
        public abstract void Dispose();

        /// <summary>
        /// Uses the current network credentials to authenticate the current request
        /// </summary>
        /// <returns>A string which is basically a token that will be used for all subsequent  request
        /// This is retrieved from the authorization header
        /// </returns>
        public abstract Parameter Authenticate();

        public Parameter AuthenticationToken { get; protected set; }

        public bool Authenticated { get; protected set; }

        public NetworkCredential Credentials { get; protected set; }

        public Parameter AuthorizationHeader { get; protected set; }
    }
}
