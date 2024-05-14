using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SupportedUserType
    {
        public SupportedUserType()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string UserType { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
