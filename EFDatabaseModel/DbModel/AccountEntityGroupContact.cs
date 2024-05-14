using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntityGroupContact
    {
        public long Id { get; set; }
        public long AccountEntityGroupId { get; set; }
        public long AccountEntityContactId { get; set; }

        public virtual AccountEntityContact AccountEntityContact { get; set; }
        public virtual AccountEntityGroup AccountEntityGroup { get; set; }
    }
}
