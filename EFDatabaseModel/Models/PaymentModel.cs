namespace EFDatabaseModel.Models
{
    public class PaymentModel
    {
        public double? Amount { get; set; }
        public string AccountNumber { get; set; }

        public string ExternalReference { get; set; }
        public string? BaseCurrency { get; set; }
        public string? QuoteCurrency { get; set; }
    }
}
