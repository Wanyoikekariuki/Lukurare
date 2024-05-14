using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SubAccountBranch
    {
        public SubAccountBranch()
        {
            AccountEntities = new HashSet<AccountEntity>();
            UserBranches = new HashSet<UserBranch>();
        }

        public long Id { get; set; }
        public string BranchName { get; set; }
        public long SubAccountId { get; set; }
        public string BuildingAddress { get; set; }
        public string PostalAddress { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string BranchCode { get; set; }

        public virtual SubAccount SubAccount { get; set; }
        public virtual ICollection<AccountEntity> AccountEntities { get; set; }
        public virtual ICollection<UserBranch> UserBranches { get; set; }
    }
}
