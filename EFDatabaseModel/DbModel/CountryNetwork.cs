using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class CountryNetwork
    {
        public CountryNetwork()
        {
            AppliedCountryNetworkSenders = new HashSet<AppliedCountryNetworkSender>();
            CountryNetworkPrefixes = new HashSet<CountryNetworkPrefix>();
            SmsGatewayCostPrices = new HashSet<SmsGatewayCostPrice>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string NetworkName { get; set; }
        public string Mcc { get; set; }
        public string Mnc { get; set; }
        public string MccMnc { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<AppliedCountryNetworkSender> AppliedCountryNetworkSenders { get; set; }
        public virtual ICollection<CountryNetworkPrefix> CountryNetworkPrefixes { get; set; }
        public virtual ICollection<SmsGatewayCostPrice> SmsGatewayCostPrices { get; set; }
    }
}
