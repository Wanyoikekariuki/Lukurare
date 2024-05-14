using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntityGroup
    {
        public AccountEntityGroup()
        {
            AccountEntityGroupContacts = new HashSet<AccountEntityGroupContact>();
        }

        public long Id { get; set; }
        public string GroupName { get; set; }
        public long AccountEntityId { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual ICollection<AccountEntityGroupContact> AccountEntityGroupContacts { get; set; }
    }
}
