using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class BillReferenceServiceType
    {
        public BillReferenceServiceType()
        {
            AccountEntityContacts = new HashSet<AccountEntityContact>();
            MfsServiceCommissionDefaults = new HashSet<MfsServiceCommissionDefault>();
            MfsServiceCommissions = new HashSet<MfsServiceCommission>();
        }

        public int Id { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }

        public virtual ICollection<AccountEntityContact> AccountEntityContacts { get; set; }
        public virtual ICollection<MfsServiceCommissionDefault> MfsServiceCommissionDefaults { get; set; }
        public virtual ICollection<MfsServiceCommission> MfsServiceCommissions { get; set; }
    }
}
