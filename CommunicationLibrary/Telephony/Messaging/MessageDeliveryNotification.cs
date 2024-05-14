namespace CommunicationLibrary.Telephony.Messaging
{
    /// <summary>
    /// Indicated the wether the requesting sender of the message requires anotification when the messages gets delivered or fails to get delivered
    /// </summary>
    public enum MessageDeliveryNotification
    {
        NotifyOnSuccess = 0,
        DontNotify = 1
    }
}
