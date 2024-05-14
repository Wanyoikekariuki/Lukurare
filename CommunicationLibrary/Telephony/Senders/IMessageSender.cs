using CommunicationLibrary.Address;
using CommunicationLibrary.Telephony.Messaging;
using CommunicationLibrary.Validation;
using ProjectBase.Repository.Transaction;
using System.Collections.Generic;

namespace CommunicationLibrary.Telephony.Senders
{
    public interface IMessageSender<
        SentMessageType,
        AddrressType,
        AddressValueType,
        SentMessageBodyType,
        SentMessageResultType
    > : IValidatable, Security.Credentials.IAuthenticable
        where SentMessageBodyType : class
        where AddrressType : class, ICommunicationAddress<AddressValueType>
        where SentMessageResultType : class
        where AddressValueType : class
        where SentMessageType : IMessage<SentMessageBodyType, AddrressType, AddressValueType>
    {
        /// <summary>
        /// The method that does the actuall sending of the messages
        /// </summary>
        /// <returns>A custom execution result that contains Sent Boolen or IsOkay to show if the message was send</returns>
        ExecutionResult<IEnumerable<SentMessageResultType>> SendMessage();

        /// <summary>
        /// The actual message to send
        /// </summary>
        IEnumerable<SentMessageType> MessageToSend { get; set; }
    }
}
