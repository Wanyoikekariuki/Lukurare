using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntityType
    {
        public AccountEntityType()
        {
            AccountEntities = new HashSet<AccountEntity>();
            AccountEntityTypeAdditionalDetails = new HashSet<AccountEntityTypeAdditionalDetail>();
            AccountEntityTypeRequiredDocuments = new HashSet<AccountEntityTypeRequiredDocument>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Prefix { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<AccountEntity> AccountEntities { get; set; }
        public virtual ICollection<AccountEntityTypeAdditionalDetail> AccountEntityTypeAdditionalDetails { get; set; }
        public virtual ICollection<AccountEntityTypeRequiredDocument> AccountEntityTypeRequiredDocuments { get; set; }
    }
}
