using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MfsCurrencyType
    {
        public MfsCurrencyType()
        {
            MfsCurrencyConversionBaseCurrencies = new HashSet<MfsCurrencyConversion>();
            MfsCurrencyConversionQuoteCurrencies = new HashSet<MfsCurrencyConversion>();
            MfsEntityAccounts = new HashSet<MfsEntityAccount>();
        }

        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }

        public virtual ICollection<MfsCurrencyConversion> MfsCurrencyConversionBaseCurrencies { get; set; }
        public virtual ICollection<MfsCurrencyConversion> MfsCurrencyConversionQuoteCurrencies { get; set; }
        public virtual ICollection<MfsEntityAccount> MfsEntityAccounts { get; set; }
    }
}
