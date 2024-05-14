using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectBase.Repository.Configuration
{
    public static class DefaultConfiguration
    {
        public static class Accounts
        {
            public static string AccountName = "Float";
            public static string RetainerAccount = "Retainer Fee";
            public static string ApplicationAccount = "Application Fee";
            public static string IELTSAccount = "IELTS ";
            public static string ECAAccount = "ECA";
            public static string FinalDeductionAccount = "Final Deduction";
        }

        public static class Currency
        {
            public static string defaultCurrency = "KES";
        }

        public static class Gender
        {
            public static string OtherGenderName = "Other";
        }

        public static class IdentificationDocumentType
        {
            public static string MobilePhoneDocumentName = "Phone";
            public static string NationalIDDocumentName = "National ID";
        }

        public static class AccountEntityTypeAditionalDetails
        {
            public static string NationaIDNumber = "ID No";
            public static string ParentIDNUmber = "Parent ID No";
            public static string BirthCertificateNumber = "Birth Certificate No";
            public static string ChildBirthCertificateNumber = "Childs Birth Certificate No";
            public static string SpouceIDNumber = "Spouse ID No";
            public static string GoodConductNumber = "Good Conduct No";
            public static string KraPinNumber = "KRA Pin No";
            public static string PassportNumber = "PassPort No";
            public static string CountryOfOrigin = "Country of origin";
            public static string Citizenship = "Citizenship";
            public static string CountryToTravel = "Country to Travel";
            public static string CurrentCountry = "Current Country";
            public static string Age = "Age";
            public static string HighestLevelOfEducation = "Highest level of education";
            public static string HowDidYouKnowOfUs = "How did you know of us";
        }

        public static class SystemModule
        {
            public static string TopupModuleName = "BillReferenceServiceRequest";
        }

        public static class AccountActiveState
        {
            public static string ActiveAccountStateName = "Active";
        }

        public static class SupportedUserType
        {
            public static string WebUserType = "Web";
        }

        
            public static class MfsAccountTypeTransactionAdditionalDetails
            {
               
                public static string currentMeterReadingKeyName = "Current Meter Reading";
                public static string addedUnitsKeyName = "Added Units";
                public static string closeAtReadingKeyName = "Close At Reading";
                public static string meterOpenedKeyName = "Meter Opened";
                public static string outstandingUnitsKeyName = "Outstanding Units";
               
                
        }
       public static class MfsAccountOperationType
        {
            public static string supportsMeterOperationsOperationName = "Supports Meter Operations";
            public static string supportsTenantOperationsOperationName = "Supports Tenant Operations";
        }
            public static class MfsAccountPaymode
        {
            public static string cashPaymodeName = "Cash";
            public static string bankPaymodeName = "Bank";
            public static string mpesaPaymodeName = "Mpesa";
        }

            public static class MfsCurrencyType
        {
            
          
            public static string quoteCurrencyCode = "Cubic_Metres";
        }

        public static class MfsAccountTypeName
        {
            public static string rentAccountTypeName = "Rent Account";
            public static string waterMeterAccountTypeName = "Water Meter";
            public static string defaultAccountTypeName = "Float";
            public static string retainerAccountTypeName = "Retainer Fee";
            public static string applicationAccountTypeName = "Application Fee";
            public static string IELTSAccountTypeName = "IELTS ";
            public static string ECAAccountTypeName = "ECA";
            public static string FinalDeductionAccountTypeName = "Final Deduction";
        }

        public static class AccountEntityType
        {
            public static string customerTypeName = "Customer";
            public static string agentTypeName = "Agent";
            public static string adminTypeName = "Admin";
            public static string InternTypeName = "Intern";
            public static string companyTypeName = "Company";

        }

        public static class ProjectApplicationStatus
        {
            public static string acceptedTypeName = "Accepted";
            public static string notReviewedTypeName = "Not Reviewed";
            public static string rejectedTypeName = "Rejected";          

        }

        public static class Systemroles
        {
            public static string roleName = "Admin";
        }

        public static class MfsEntityAccountTypeOperation
        {
            public static double defaultRentAmount = 200;
        }

        public static bool IsEndMonth(DateTime currentDate)
        {
            if (currentDate.AddDays(1).Month != currentDate.Month)
            {
                return true;
            }
            else
                return false;
        }



    }
}
