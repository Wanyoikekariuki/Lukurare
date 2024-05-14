using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class TicketingTicketStatus
    {
        public TicketingTicketStatus()
        {
            TicketingTickets = new HashSet<TicketingTicket>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }

        public virtual ICollection<TicketingTicket> TicketingTickets { get; set; }
    }
}
