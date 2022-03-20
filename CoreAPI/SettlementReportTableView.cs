using System;

namespace CoreAPI
{
    public class SettlementReportTableView
    {
        public string ReportName { get; set; }

        public double BatchNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double TotalPayout { get; set; }

        public double TotalAdyenSum { get; set; }

        public SettlementReportTableView()
        {

        }

        public SettlementReportTableView(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
