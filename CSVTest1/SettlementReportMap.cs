using CsvHelper.Configuration;

namespace CSVTest1
{
    public class SettlementReportMap : ClassMap<SettlementReport>
    {
        public SettlementReportMap()
        {
            Map(m => m.CompanyAccount).Name("Company Account");
            Map(m => m.MerchantAccount).Name("Merchant Account");
            Map(m => m.PspReference).Name("Psp Reference");
            Map(m => m.MerchantReference).Name("Merchant Reference");
            Map(m => m.PaymentMethod).Name("Payment Method");
            Map(m => m.CreationDate).Name("Creation Date");
            Map(m => m.TimeZone).Name("TimeZone");
            Map(m => m.Type).Name("Type");
            Map(m => m.ModificatinReference).Name("Modification Reference");
            Map(m => m.GrossCurrency).Name("Gross Currency");
            Map(m => m.GrossDebitGC).Name("Gross Debit (GC)");
            Map(m => m.GrossCreditGC).Name("Gross Credit (GC)");
            Map(m => m.ExchangeRate).Name("Exchange Rate");
            Map(m => m.NetCurrency).Name("Net Currency");
            Map(m => m.NetDebitNC).Name("Net Debit (NC)");
            Map(m => m.NetCreditNC).Name("Net Credit (NC)");
            Map(m => m.CommissionNC).Name("Commission (NC)");
            Map(m => m.MarkupNC).Name("Markup (NC)");
            Map(m => m.SchemeFeesNC).Name("Scheme Fees (NC)");
            Map(m => m.InterchangeNC).Name("Interchange (NC)");
            Map(m => m.PaymentMethodVariant).Name("Payment Method Variant");
            Map(m => m.ModificationMerchantReference).Name("Modification Merchant Reference");
            Map(m => m.BatchNumber).Name("Batch Number");
            Map(m => m.AdvancementCode).Name("Advancement Code");
        }
    }
}
