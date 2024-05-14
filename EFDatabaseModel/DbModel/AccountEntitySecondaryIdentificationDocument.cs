using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntitySecondaryIdentificationDocument
    {
        public int Id { get; set; }
        public long AccountEntityId { get; set; }
        public int RequiredDocumentsId { get; set; }
        public string Path { get; set; }
        public bool Validated { get; set; }
        public bool Active { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual AccountEntityTypeRequiredDocument RequiredDocuments { get; set; }
    }
}
