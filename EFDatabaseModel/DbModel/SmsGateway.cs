using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SmsGateway
    {
        public SmsGateway()
        {
            SmsGatewayCostPrices = new HashSet<SmsGatewayCostPrice>();
        }

        public int Id { get; set; }
        public string GatewayName { get; set; }
        public bool IsOurGateway { get; set; }

        public virtual ICollection<SmsGatewayCostPrice> SmsGatewayCostPrices { get; set; }
    }
}
