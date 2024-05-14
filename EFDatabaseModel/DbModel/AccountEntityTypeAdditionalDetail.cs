using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntityTypeAdditionalDetail
    {
        public AccountEntityTypeAdditionalDetail()
        {
            AccountEntityTypeAdditionalDetailsValues = new HashSet<AccountEntityTypeAdditionalDetailsValue>();
        }

        public int Id { get; set; }
        public int AccountEntityTypeId { get; set; }
        public string KeyName { get; set; }
        public bool Required { get; set; }
        public string DataType { get; set; }

        public virtual AccountEntityType AccountEntityType { get; set; }
        public virtual ICollection<AccountEntityTypeAdditionalDetailsValue> AccountEntityTypeAdditionalDetailsValues { get; set; }
    }
}
