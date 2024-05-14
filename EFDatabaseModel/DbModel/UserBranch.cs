using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class UserBranch
    {
        public long Id { get; set; }
        public long SubAccountBranchId { get; set; }
        public long UserId { get; set; }
        public bool IsActive { get; set; }

        public virtual SubAccountBranch SubAccountBranch { get; set; }
        public virtual User User { get; set; }
    }
}
