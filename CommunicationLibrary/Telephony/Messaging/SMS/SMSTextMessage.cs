using CommunicationLibrary.Telephony.Address;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunicationLibrary.Telephony.Messaging.SMS
{
    /// <summary>
    /// Abstracts a Text/SMS message functionality and requirements
    /// </summary>
    public class SMSTextMessage : Message<string, PhoneNumber, string>
    {
        public SMSTextMessage() { }

        public SMSTextMessage(
            string smsMessage,
            string _phoneNumber,
            string sender = "",
            bool messageCanBeEmpty = false,
            MessageType messageType = MessageType.PlainText,
            MessageDeliveryNotification messageDeliveryNotification =
                MessageDeliveryNotification.DontNotify,
            bool throwOnException = false
        )
            : base(
                smsMessage,
                new HashSet<PhoneNumber> { new PhoneNumber(_phoneNumber) },
                sender,
                messageCanBeEmpty,
                messageType,
                messageDeliveryNotification,
                throwOnException
            ) { }

        public SMSTextMessage(
            string smsMessage,
            string sender,
            IEnumerable<string> _phoneNumbers,
            bool messageCanBeEmpty = false,
            MessageType messageType = MessageType.PlainText,
            MessageDeliveryNotification messageDeliveryNotification =
                MessageDeliveryNotification.DontNotify,
            bool throwOnException = false
        )
            : base(
                smsMessage,
                _phoneNumbers.Select(r => new PhoneNumber(r)),
                sender,
                messageCanBeEmpty,
                messageType,
                messageDeliveryNotification,
                throwOnException
            ) { }

        /// <summary>
        /// Gives the size of an SMS body check the page for more details
        /// In this case what is returned is the number of pages that make up the SMS
        /// http://www.autotext.com.au/press-release/how-many-characters-sms-message
        /// </summary>
        /// <returns>An integer indicating the size of the message and -1 incase an error occured while attempting to get message size</returns>
        public override int GetBodySize()
        {
            int bodySize = 0;
            if (IsValid() == false)
                return -1;

            if (Body.Length >= 0 && Body.Length <= 160)
                bodySize = 1;
            else if (Body.Length >= 161 && Body.Length <= 306)
                bodySize = 2;
            else if (Body.Length >= 307 && Body.Length <= 459)
                bodySize = 3;
            else if (Body.Length >= 500 && Body.Length <= 621)
                bodySize = 4;
            else
                return -1;

            return bodySize;
        }

        /// <summary>
        /// Validates the SMS message to send
        /// It also validates the phone numbers i.e. addresses to send the current message to
        /// If both the current message and all it targeted destination are valid then the message is valid
        /// Throws invalid operation exception is ThrowOnException is true and Validation failed
        /// </summary>
        /// <returns>Returns a boolean value indicating wether the current Message is valid</returns>
        public override bool IsValid()
        {
            bool isvalid = base.IsValid(); //validates the address
            if (isvalid == false)
                return isvalid;

            if (string.IsNullOrEmpty(Body) && MessageCanBeEmpty == false)
            {
                ExceptionMessage = string.Format(
                    "The supplied message is empty and AllowEmptyMessages has been set to false"
                );
                isvalid = false;
            }
            else if (Body.Length > 621)
            {
                ExceptionMessage = string.Format(
                    "The maximum SMS body size is 621\nSupplied message of size {0} is greater than this limit",
                    Body.Length
                );
                isvalid = false;
            }

            if (isvalid == false && ThrowOnException == true)
                throw new InvalidOperationException(ExceptionMessage);
            return isvalid;
        }

        /// <summary>
        /// Converts the current SMS message to a string
        /// </summary>
        /// <returns>A string representation of the current address</returns>
        public override string BodyToString()
        {
            return Body;
        }
    }
}
