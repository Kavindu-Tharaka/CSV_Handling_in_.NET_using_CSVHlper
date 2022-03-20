using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VoucherAPI
{
    public class SettlementReportTableView
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [JsonConstructor]
        public SettlementReportTableView(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
