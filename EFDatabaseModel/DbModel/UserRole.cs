using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class UserRole
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long SystemRoleId { get; set; }
        public bool Active { get; set; }

        public virtual SystemRole SystemRole { get; set; }
        public virtual User User { get; set; }
    }
}
