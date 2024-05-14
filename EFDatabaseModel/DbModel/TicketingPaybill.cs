using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class TicketingPaybill
    {
        public TicketingPaybill()
        {
            TicketingTickets = new HashSet<TicketingTicket>();
        }

        public long Id { get; set; }
        public string PaybillNumber { get; set; }
        public long MerchantId { get; set; }

        public virtual TicketingMerchant Merchant { get; set; }
        public virtual ICollection<TicketingTicket> TicketingTickets { get; set; }
    }
}
