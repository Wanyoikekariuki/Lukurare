using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class Gender
    {
        public Gender()
        {
            AccountEntities = new HashSet<AccountEntity>();
        }

        public int Id { get; set; }
        public string GenderName { get; set; }
        public string Abbreviation { get; set; }

        public virtual ICollection<AccountEntity> AccountEntities { get; set; }
    }
}
