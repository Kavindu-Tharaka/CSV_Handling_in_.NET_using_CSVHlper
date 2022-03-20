using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVTest1
{
    public class SettlementReportTableView
    {
        public string ReportName { get; set; }

        public double BatchNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double TotalPayout { get; set; }

        public double TotalAdyenSum { get; set; }

    }
}
