using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class UserUpdateLog
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long BackUserIdLog { get; set; }
        public long SystemTransactionId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool Approved { get; set; }
        public bool Cancelled { get; set; }

        public virtual User User { get; set; }
    }
}
