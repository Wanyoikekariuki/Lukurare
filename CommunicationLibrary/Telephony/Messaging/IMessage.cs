using CommunicationLibrary.Address;
using CommunicationLibrary.Validation;
using System.Collections.Generic;

namespace CommunicationLibrary.Telephony.Messaging
{
    /// <summary>
    /// Gives a defination of the behavior of messages as expected by this communication client
    /// </summary>
    public interface IMessage<BodyType, AddressType, AddressValueType> : IValidatable
        where BodyType : class
        where AddressType : class, ICommunicationAddress<AddressValueType>
        where AddressValueType : class
    {
        /// <summary>
        /// This represents the mask of sending organization or the phone number of the sender
        /// </summary>
        string Sender { get; set; }

        /// <summary>
        /// The intended destination for the current message
        /// </summary>
        IEnumerable<AddressType> DestinationAddress { get; set; }

        /// <summary>
        /// The body of the message
        /// </summary>
        BodyType Body { get; set; }

        /// <summary>
        /// Indicates the type of the current message
        /// </summary>
        MessageType MessageType { get; set; }

        /// <summary>
        /// Indicates wether the requesting clients need to notified incase this message is delivered
        /// </summary>
        MessageDeliveryNotification MessageDeliveryNotification { get; set; }

        /// <summary>
        /// Get or set a value indicating wether we should this message to be still valid even when empty
        /// </summary>
        bool MessageCanBeEmpty { get; set; }

        /// <summary>
        /// To ensure that each message must have a body size
        /// </summary>
        /// <returns>An integer that give the size of the message</returns>
        int GetBodySize();

        /// <summary>
        /// Returns the string representation of the body of the message a specific type
        /// </summary>
        /// <returns></returns>
        string BodyToString();

        /// <summary>
        /// A value that indicates wether the current message is a broadcash message or not
        /// //determined at constructor level by the size of the IEnumarable that hold message list to sent
        /// >1 the true else false
        /// </summary>
        bool BroadCastMessage { get; }
    }
}
