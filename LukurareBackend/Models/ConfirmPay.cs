using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models
{
    public class ConfirmPay
    {
        public string transactionReference { get; set; }
        public bool newPostedStatus { get; set; }
    }
}
