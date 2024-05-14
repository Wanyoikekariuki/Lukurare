using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntityTypeRequiredDocument
    {
        public AccountEntityTypeRequiredDocument()
        {
            AccountEntitySecondaryIdentificationDocuments = new HashSet<AccountEntitySecondaryIdentificationDocument>();
        }

        public int Id { get; set; }
        public int AccountEntityTypeId { get; set; }
        public int IdentificationDocumentTypeId { get; set; }
        public bool Required { get; set; }

        public virtual AccountEntityType AccountEntityType { get; set; }
        public virtual IdentificationDocumentType IdentificationDocumentType { get; set; }
        public virtual ICollection<AccountEntitySecondaryIdentificationDocument> AccountEntitySecondaryIdentificationDocuments { get; set; }
    }
}
