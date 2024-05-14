using System;

namespace CommunicationLibrary.Address
{
    /// <summary>
    /// Base class for all communication addresses
    /// Will wrap any functionalit that applies to all communication addresses
    /// </summary>
    /// <typeparam name="AddressType"></typeparam>
    public abstract class CommunicationAddress<AddressType> : ICommunicationAddress<AddressType>
        where AddressType : class
    {
        /// <summary>
        /// Gets or sets a value for  the current address
        /// </summary>
        public AddressType SuppliedAddress { get; set; }

        /// <summary>
        /// Gets a value for the normalized address based on current address
        /// </summary>
        public abstract AddressType NormalizedAddress { get; }

        /// <summary>
        /// Get or set the exception message associated with the current IMessage
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets a boolean values that indicates wether or not we should throw on exception
        /// </summary>
        public bool ThrowOnException { get; set; }

        public CommunicationAddress(AddressType address, string uniqueMessageIdentifier)
        {
            SuppliedAddress = address;
            UniqueMessageIdentifier = uniqueMessageIdentifier;
        }

        public CommunicationAddress() { }

        /// <summary>
        /// Validates to check if the current address is valid
        /// </summary>
        /// <returns>A boolean value of true or false indicating wether the address is valid or not</returns>
        public abstract bool IsValid();

        /// <summary>
        /// Gets the string representation of the either the SuppliedAddress or NormalizedAddress based on supplied value for returnNormalizedAddress
        /// </summary>
        /// <param name="returnNormalizedAddress">A boolean value indicating wether to return the normalized or supplied address</param>
        /// <returns></returns>
        public abstract string GetAddressAsString(bool returnNormalizedAddress = true);

        /// <summary>
        /// Get the value returned by GetAddressString() function
        /// Ovveride Object.ToString()
        /// </summary>
        /// <returns>A string value of the normalized address</returns>
        public override string ToString()
        {
            return GetAddressAsString();
        }

        /// <summary>
        /// Get ther value returned by GetAddressString() function
        /// </summary>
        /// <param name="returnNormalizedAddress">A boolean values that determines wether we print the normalized address or supplied raw address</param>
        /// <returns>A string value </returns>
        public string ToString(bool returnNormalizedAddress)
        {
            return GetAddressAsString(returnNormalizedAddress);
        }

        private string uniqueMessageIdentifier;

        /// <summary>
        /// The unique id that identifies a message from the client application
        /// Very important when u submit a list of batched messages to the gate way server
        /// When -1 is supplied then it is we ingore
        /// </summary>
        public string UniqueMessageIdentifier
        {
            get
            {
                if (string.IsNullOrEmpty(uniqueMessageIdentifier))
                    uniqueMessageIdentifier = Guid.NewGuid().ToString();
                return uniqueMessageIdentifier;
            }
            set { uniqueMessageIdentifier = value; }
        }
    }
}
