using CsvHelper.Configuration;

namespace CoreAPI
{
    public class SettlementReportTableViewMap : ClassMap<SettlementReportTableView>
    {
        public SettlementReportTableViewMap()
        {
            Map(m => m.ReportName).Index(0).Name("Report Name");
            Map(m => m.BatchNumber).Index(1).Name("Batch Number");
            Map(m => m.StartDate).Index(2).Name("Start Date");
            Map(m => m.EndDate).Index(3).Name("End Date");
            Map(m => m.TotalPayout).Index(4).Name("Total Payout");
            Map(m => m.TotalAdyenSum).Index(5).Name("Total Adyen Sum");
        }
    }
}
