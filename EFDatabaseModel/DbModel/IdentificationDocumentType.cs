using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class IdentificationDocumentType
    {
        public IdentificationDocumentType()
        {
            AccountEntities = new HashSet<AccountEntity>();
            AccountEntityTypeRequiredDocuments = new HashSet<AccountEntityTypeRequiredDocument>();
        }

        public int Id { get; set; }
        public string DocumentName { get; set; }

        public virtual ICollection<AccountEntity> AccountEntities { get; set; }
        public virtual ICollection<AccountEntityTypeRequiredDocument> AccountEntityTypeRequiredDocuments { get; set; }
    }
}
