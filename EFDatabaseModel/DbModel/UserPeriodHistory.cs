using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class UserPeriodHistory
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime RenewalDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsRenewed { get; set; }
        public string OldPassword { get; set; }

        public virtual User User { get; set; }
    }
}
