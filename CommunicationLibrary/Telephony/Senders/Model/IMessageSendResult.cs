namespace CommunicationLibrary.Telephony.Senders.Model
{
    public interface IMessageSendResult
    {
        /// <summary>
        /// A boolean indicates wether the message was sent orr not
        /// </summary>
        bool Sent { get; set; }

        /// <summary>
        /// A boolean indicates wether the message was sent orr not
        /// </summary>
        bool Delivered { get; }

        /// <summary>
        /// A template type that represents the unique id of the sent message as stored on the gateway server
        /// </summary>
        string GateWayUniqueMessageId { get; set; }

        /// <summary>
        /// The cost assoctiated with sending the message
        /// </summary>
        double MessageCost { get; set; }

        /// <summary>
        /// Will represent the unique ID of the message assigned by the client application,expected also to passed to server along request
        /// </summary>
        string ClientSentMessageUniqueId { get; set; }

        /// <summary>
        /// A message describition what happened to this message on the server
        /// </summary>
        string StatusText { get; set; }

        /// <summary>
        /// This is where the message was sent to i.e the number that was a target for the message
        /// </summary>
        string Destination { get; set; }

        /// <summary>
        /// The response code that was returned by the gateway server
        /// </summary>
        string ResponseCode { get; set; }
    }
}
