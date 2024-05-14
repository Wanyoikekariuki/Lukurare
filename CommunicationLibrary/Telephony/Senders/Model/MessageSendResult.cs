namespace CommunicationLibrary.Telephony.Senders.Model
{
    public abstract class MessageSendResult : IMessageSendResult
    {
        public MessageSendResult() { }

        /// <summary>
        /// A template type that represents the unique id of the sent message
        /// </summary>
        public string GateWayUniqueMessageId { get; set; }

        /// <summary>
        /// The cost assoctiated with sending the message
        /// </summary>
        public double MessageCost { get; set; }

        /// <summary>
        /// Will represent the unique ID of the message assigned by the client application and passed along this request to the gateway server
        /// </summary>
        public string ClientSentMessageUniqueId { get; set; }

        /// <summary>
        /// A boolean indicates wether the message was sent orr not
        /// </summary>
        public bool Sent { get; set; }

        /// <summary>
        /// A boolean indicates wether the message was sent orr not
        /// </summary>
        public bool Delivered
        {
            get { return Sent; }
        }

        /// <summary>
        /// A message describition what happened to this message on the server
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// This is where the message was sent to or was to be sent to incase message sending failed it falied
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// The response code that was returned by the gateway server
        /// </summary>
        public string ResponseCode { get; set; }
    }
}
