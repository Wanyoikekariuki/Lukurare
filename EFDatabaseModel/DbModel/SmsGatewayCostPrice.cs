using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SmsGatewayCostPrice
    {
        public SmsGatewayCostPrice()
        {
            SmsRatePlanTemplates = new HashSet<SmsRatePlanTemplate>();
        }

        public long Id { get; set; }
        public int SmsGatewayId { get; set; }
        public int CountryNetworkId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool OverwriteExistingPrice { get; set; }
        public float CostPrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeactivatedDate { get; set; }

        public virtual CountryNetwork CountryNetwork { get; set; }
        public virtual SmsGateway SmsGateway { get; set; }
        public virtual ICollection<SmsRatePlanTemplate> SmsRatePlanTemplates { get; set; }
    }
}
