using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class TicketingTicket
    {
        public TicketingTicket()
        {
            TicketingTechnicianTicketTimes = new HashSet<TicketingTechnicianTicketTime>();
        }

        public long Id { get; set; }
        public long? InitiatingUserId { get; set; }
        public long TargetingAccountEntityId { get; set; }
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public int TicketStatusId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateClosed { get; set; }
        public string Description { get; set; }
        public int Complexity { get; set; }
        public byte IsUrgent { get; set; }
        public string Notes { get; set; }
        public string ReferenceNumber { get; set; }
        public string PhoneNo { get; set; }
        public long TicketingPaybillId { get; set; }

        public virtual User InitiatingUser { get; set; }
        public virtual AccountEntity TargetingAccountEntity { get; set; }
        public virtual TicketingTicketStatus TicketStatus { get; set; }
        public virtual TicketingPaybill TicketingPaybill { get; set; }
        public virtual ICollection<TicketingTechnicianTicketTime> TicketingTechnicianTicketTimes { get; set; }
    }
}
