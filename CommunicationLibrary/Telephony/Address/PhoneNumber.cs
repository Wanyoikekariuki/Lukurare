using CommunicationLibrary.Address;
using System;

namespace CommunicationLibrary.Telephony.Address
{
    /// <summary>
    /// Abstracts a PhoneNumber using from the CommunicationAddress API format
    /// </summary>
    public class PhoneNumber : CommunicationAddress<string>
    {
        public PhoneNumber() { }

        public PhoneNumber(string phoneNumber, string uniqueMessageIdentifier = null)
            : base(phoneNumber, uniqueMessageIdentifier) { }

        /// <summary>
        /// Provide some logic to validate a phone contact
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            bool isValid = true;
            #region validation code here and value set to isValid
            //Regex regex = new Regex("^[0]");
            //Match match = regex.Match(SuppliedAddress);
            //if (match.Success == false)
            //{
            //    isValid = false;
            //    ExceptionMessage = string.Format("The phone number {0} is not valid", SuppliedAddress);
            //}
            #endregion
            if (isValid == false && ThrowOnException == true)
                throw new InvalidOperationException(ExceptionMessage);
            return isValid;
        }

        public string GetDefaultCountryCode()
        {
            return "254";
        }

        /// <summary>
        /// Returns the phone number as a string
        /// </summary>
        /// <param name="returnNormalizedAddress">An optional boolean to determine the returned string representation wether it's for the normalized or supplied address
        /// Default value is true meaning it always returns the normalized address unless otherwise specified</param>
        /// <returns>A string representation of the current phone number</returns>
        public override string GetAddressAsString(bool returnNormalizedAddress = true)
        {
            if (returnNormalizedAddress)
                return NormalizedAddress;
            else
                return SuppliedAddress;
        }

        /// <summary>
        /// will normalize a phone number and default to +254 incase phone number begins with 0
        /// </summary>
        public override string NormalizedAddress
        {
            get
            {
                var stringToReturn = SuppliedAddress;
                if (stringToReturn == null)
                    throw new ArgumentNullException("address");
                stringToReturn = stringToReturn.Trim();
                if (stringToReturn.StartsWith("0", StringComparison.OrdinalIgnoreCase))
                    stringToReturn = string.Format(
                        "{0}{1}",
                        GetDefaultCountryCode(),
                        stringToReturn.Substring(1)
                    );
                return stringToReturn;
            }
        }
    }
}
