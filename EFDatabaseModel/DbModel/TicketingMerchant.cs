using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class TicketingMerchant
    {
        public TicketingMerchant()
        {
            TicketingDealers = new HashSet<TicketingDealer>();
            TicketingPaybills = new HashSet<TicketingPaybill>();
        }

        public long Id { get; set; }
        public string MerchantName { get; set; }
        public string Prefix { get; set; }

        public virtual ICollection<TicketingDealer> TicketingDealers { get; set; }
        public virtual ICollection<TicketingPaybill> TicketingPaybills { get; set; }
    }
}
