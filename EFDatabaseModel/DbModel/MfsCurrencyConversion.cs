using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MfsCurrencyConversion
    {
        public int Id { get; set; }
        public int BaseCurrencyId { get; set; }
        public int QuoteCurrencyId { get; set; }
        public double BidBuyingPrice { get; set; }
        public double AskSellingPrice { get; set; }
        public double CurrentExchangeRate { get; set; }

        public virtual MfsCurrencyType BaseCurrency { get; set; }
        public virtual MfsCurrencyType QuoteCurrency { get; set; }
    }
}
