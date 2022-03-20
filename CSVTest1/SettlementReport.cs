namespace CSVTest1
{
    public class SettlementReport
    {
        public string FilePath { get; set; }

        public string CompanyAccount { get; set; }

        public string MerchantAccount { get; set; }

        public string PspReference { get; set; }

        public string MerchantReference { get; set; }

        public string PaymentMethod { get; set; }

        public string CreationDate { get; set; }

        public string TimeZone { get; set; }

        public string Type { get; set; }

        public string ModificatinReference { get; set; }

        public string GrossCurrency { get; set; }

        public string GrossDebitGC { get; set; }

        public string GrossCreditGC { get; set; }

        public string ExchangeRate { get; set; }

        public string NetCurrency { get; set; }

        public string NetDebitNC { get; set; }

        public string NetCreditNC { get; set; }

        public string CommissionNC { get; set; }

        public string MarkupNC { get; set; }

        public string SchemeFeesNC { get; set; }

        public string InterchangeNC { get; set; }

        public string PaymentMethodVariant { get; set; }

        public string ModificationMerchantReference { get; set; }

        public int BatchNumber { get; set; }

        public string AdvancementCode { get; set; }
    }
}
