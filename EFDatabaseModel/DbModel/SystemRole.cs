using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SystemRole
    {
        public SystemRole()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public long Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
