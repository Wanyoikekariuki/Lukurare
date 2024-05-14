using CommunicationLibrary.Address;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunicationLibrary.Telephony.Messaging.SMS
{
    /// <summary>
    /// The base class of any message that can be send via this communication client API
    /// If there is some functionality that applies to all messages in the same way exactly it can wrapped or introduced into this class
    /// </summary>
    /// <typeparam name="BodyType">A template represent the what makes up the body of the said message e.g for sms message body would be a string</typeparam>
    public abstract class Message<BodyType, AddressType, AddressValueType>
        : IMessage<BodyType, AddressType, AddressValueType>
        where BodyType : class
        where AddressType : class, ICommunicationAddress<AddressValueType>
        where AddressValueType : class
    {
        public DateTime? MessageScheduleDate { get; set; }

        /// <summary>
        /// The intended destination for the current message
        /// </summary>
        public IEnumerable<AddressType> DestinationAddress { get; set; }

        /// <summary>
        /// The body of the message
        /// </summary>
        public BodyType Body { get; set; }

        /// <summary>
        /// Indicates the type of the current message
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Indicates wether the requesting clients need to notified incase this message is delivered
        /// </summary>
        public MessageDeliveryNotification MessageDeliveryNotification { get; set; }

        public Message() { }

        public Message(
            BodyType body,
            IEnumerable<AddressType> address,
            string sender,
            bool messageCanBeEmpty,
            MessageType messageType,
            MessageDeliveryNotification messageDeliveryNotification,
            bool throwOnException
        )
        {
            Body = body;
            DestinationAddress = address;
            BroadCastMessage = DestinationAddress.Count() > 1;
            MessageCanBeEmpty = messageCanBeEmpty;
            MessageType = messageType;
            MessageDeliveryNotification = messageDeliveryNotification;
            ThrowOnException = throwOnException;
            Sender = sender;
        }

        /// <summary>
        /// Get or set a value indicating wether we should this message to be still valid even when empty
        /// </summary>
        public bool MessageCanBeEmpty { get; set; }

        /// <summary>
        /// Each message must have a body size
        /// </summary>
        /// <returns>An integer that give the size of the message</returns>
        public abstract int GetBodySize();

        /// <summary>
        /// Used to check wether a specified message is valid or within acceptable valid limits of a certain type of message
        /// </summary>
        /// <returns>Boolean indicating wether the above is true</returns>
        public virtual bool IsValid()
        {
            //try validate the adresses
            //left here on validating
            bool result = true;
            if (DestinationAddress.Any() == false)
            {
                ExceptionMessage = "This message does not have any valid destination addresses";
                result = false;
            }
            var invalidAdresses = DestinationAddress.Where(r => r.IsValid() == false);
            if (invalidAdresses.Any())
            {
                ExceptionMessage = string.Format(
                    "Address validation failed\nPlease check {0} address{1}\n",
                    invalidAdresses
                        .Select(rs => rs.GetAddressAsString())
                        .Aggregate((r1, r2) => string.Format("{0},{1}", r1, r2)),
                    (invalidAdresses.Count() > 1 ? "s" : "")
                );
                result = false;
            }
            if (result == false && ThrowOnException == true)
                throw new InvalidOperationException(ExceptionMessage);
            return result;
        }

        /// <summary>
        /// Super class will ovverride this to give the string representation of a Message
        /// </summary>
        /// <returns></returns>
        public abstract string BodyToString();

        /// <summary>
        /// Returns the string representation of an object based on the value returned by BodyToString in the super classes
        /// </summary>
        /// <returns>A string representation of this message</returns>
        public override string ToString()
        {
            return BodyToString();
        }

        /// <summary>
        /// Get or set the exception message associated with the current IMessage
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets a boolean values that indicates wether or not we should throw on exception
        /// </summary>
        public bool ThrowOnException { get; set; }

        /// <summary>
        /// The adrress of the entity sending the message in string format
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// A value that indicates wether the current message is a broadcash message or not
        /// //determined at constructor level by the size of the IEnumarable that hold message list to sent
        /// >1 the true else false
        /// </summary>
        public bool BroadCastMessage { get; private set; }
    }
}
