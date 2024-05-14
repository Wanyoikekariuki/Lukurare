namespace CommunicationLibrary.Address
{
    /// <summary>
    /// ICommunicationAddress
    /// </summary>
    /// <typeparam name="AddressType"></typeparam>
    public interface ICommunicationAddress<AddressType>
        : CommunicationLibrary.Validation.IValidatable
        where AddressType : class
    {
        /// <summary>
        /// The address value after normalization
        /// </summary>
        AddressType NormalizedAddress { get; }

        /// <summary>
        /// The address value
        /// </summary>
        AddressType SuppliedAddress { get; set; }

        /// <summary>
        /// Returns the required address in it's string representation
        /// </summary>
        /// <param name="returnNormalizedAddress">An optional boolean to determine the returned string representation wether it's for the normalized or supplied address
        /// Default value is true meaning it always returns the normalized address unless otherwise specified</param>
        /// <returns>A string representation of the current phone number</returns>
        string GetAddressAsString(bool returnNormalizedAddress = true);

        /// <summary>
        /// The unique id that identifies a message from the client application
        /// Very important when u submit a list of batched messages to the gate way server
        /// When -1 is supplied then it is we ingore
        /// </summary>
        string UniqueMessageIdentifier { get; set; }
    }
}
