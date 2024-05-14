using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntityActiveState
    {
        public AccountEntityActiveState()
        {
            AccountEntities = new HashSet<AccountEntity>();
        }

        public int Id { get; set; }
        public string StateName { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<AccountEntity> AccountEntities { get; set; }
    }
}
