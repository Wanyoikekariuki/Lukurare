namespace CommunicationLibrary.Validation
{
    /// <summary>
    /// Class that represents the IValidatable interface
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Get or set the exception message associated with the current IMessage
        /// </summary>
        string ExceptionMessage { get; set; }

        /// <summary>
        /// To ensure a specified message is valid or within acceptable valid limits of a certain type of message
        /// </summary>
        /// <returns>Boolean indicating wether the above is true</returns>
        bool IsValid();

        /// <summary>
        /// Gets or sets a boolean values that indicates wether or not we should throw on exception
        /// </summary>
        bool ThrowOnException { get; set; }
    }
}
