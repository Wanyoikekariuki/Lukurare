using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LukurareBackend.Models.DashBoard
{
    public class KeyNames
    {
        public string IDNumber { get; set; }
        public string ParentIDNumber { get; set; }
        public string BirthCertificateNumber { get; set; }
        public string ChildsBirthCertificateNumber { get; set; }
        public string SpouseIDNumber { get; set; }
        public string GoodConductNumber { get; set; }
        public string KRAPinNumber { get; set; }
        public string PassPortNumber { get; set; }
        public string CountryOrigin { get; set; }
        public string CountryCitizenship { get; set; }
        public string CountryTravelling { get; set; }
        public string CurrentCountry { get; set; }
        public string Age { get; set; }
        public string HighestLevelOfEducation{ get; set; }
        public string HowDidYouKnowOfUs { get; set; }
        public string ImageDecName { get; set; }
        public string ImageDecprice { get; set; }
    }
}
