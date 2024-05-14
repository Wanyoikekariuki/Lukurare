using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models
{
   public class Payment
    {
        public long Id { get; set; }
        public string BaseCurrency { get; set; }
        public int PaymodeId { get; set; }
        public string ReferenceNumber { get; set; }
        public double Amount { get; set; }
        public string PaymentPurpose { get; set; }
        public string AccountName { get; set; }

    }
}
