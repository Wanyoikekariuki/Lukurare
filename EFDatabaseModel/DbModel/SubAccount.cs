using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SubAccount
    {
        public SubAccount()
        {
            CompanyModules = new HashSet<CompanyModule>();
            MfsAccountTypes = new HashSet<MfsAccountType>();
            SubAccountBranches = new HashSet<SubAccountBranch>();
        }

        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual SchoolMsSchool SchoolMsSchool { get; set; }
        public virtual ICollection<CompanyModule> CompanyModules { get; set; }
        public virtual ICollection<MfsAccountType> MfsAccountTypes { get; set; }
        public virtual ICollection<SubAccountBranch> SubAccountBranches { get; set; }
    }
}
