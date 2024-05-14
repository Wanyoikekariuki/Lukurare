using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class TicketingTechnicianTicketTime
    {
        public long Id { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public long? TechnicianUserId { get; set; }
        public long TicketId { get; set; }
        public string Description { get; set; }

        public virtual User TechnicianUser { get; set; }
        public virtual TicketingTicket Ticket { get; set; }
    }
}
