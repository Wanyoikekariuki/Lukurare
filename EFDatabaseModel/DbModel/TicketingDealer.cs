using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class TicketingDealer
    {
        public long Id { get; set; }
        public long DealerId { get; set; }
        public long MerchantId { get; set; }

        public virtual AccountEntity Dealer { get; set; }
        public virtual TicketingMerchant Merchant { get; set; }
    }
}
