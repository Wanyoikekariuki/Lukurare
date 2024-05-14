using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SmsRatePlanTemplate
    {
        public long Id { get; set; }
        public string TemplateName { get; set; }
        public long AccountEntityId { get; set; }
        public long SmsGatewayCostPriceId { get; set; }
        public float SellingPrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime DeactivatedDate { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual SmsGatewayCostPrice SmsGatewayCostPrice { get; set; }
    }
}
