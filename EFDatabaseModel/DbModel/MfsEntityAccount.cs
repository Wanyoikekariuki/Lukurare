using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MfsEntityAccount
    {
        public MfsEntityAccount()
        {
            MeterStates = new HashSet<MeterState>();
            MfsEntityAccountTransactions = new HashSet<MfsEntityAccountTransaction>();
            MfsServiceCommissionDefaults = new HashSet<MfsServiceCommissionDefault>();
            MfsServiceCommissions = new HashSet<MfsServiceCommission>();
        }

        public long Id { get; set; }
        public long MfsAccountTypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public long AccountEntityId { get; set; }
        public int MfsCurrencyTypeId { get; set; }
        public bool IsActive { get; set; }
        public long SystemTransactionId { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual MfsAccountType MfsAccountType { get; set; }
        public virtual MfsCurrencyType MfsCurrencyType { get; set; }
        public virtual SystemTransaction SystemTransaction { get; set; }
        public virtual ICollection<MeterState> MeterStates { get; set; }
        public virtual ICollection<MfsEntityAccountTransaction> MfsEntityAccountTransactions { get; set; }
        public virtual ICollection<MfsServiceCommissionDefault> MfsServiceCommissionDefaults { get; set; }
        public virtual ICollection<MfsServiceCommission> MfsServiceCommissions { get; set; }
    }
}
