namespace CommunicationLibrary.Telephony.Messaging
{
    /// <summary>
    /// An enumeration of the diffrent supported message type
    /// </summary>
    public enum MessageType
    {
        PlainText = 0,
        FlashMessage = 1,
        Unicode = 2,
        Reserved = 3,
        WapPushed = 4,
        PlainTextISO = 5,
        UnicodeFlash = 6,
        FlashMessageISO = 7
    }
}
