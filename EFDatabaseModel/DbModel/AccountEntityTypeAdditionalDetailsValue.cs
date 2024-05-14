using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntityTypeAdditionalDetailsValue
    {
        public int Id { get; set; }
        public int TypeAdditionalDetailsId { get; set; }
        public long AccountEntityId { get; set; }
        public string Value { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual AccountEntityTypeAdditionalDetail TypeAdditionalDetails { get; set; }
    }
}
