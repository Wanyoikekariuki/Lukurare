using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class BillReferenceServiceRequest
    {
        public long Id { get; set; }
        public long SystemTransactionId { get; set; }
        public long AccountEntityContactId { get; set; }
        public float Amount { get; set; }
        public bool Processed { get; set; }
        public bool Success { get; set; }
        public string Narration { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public int Attempts { get; set; }

        public virtual AccountEntityContact AccountEntityContact { get; set; }
        public virtual SystemTransaction SystemTransaction { get; set; }
    }
}
