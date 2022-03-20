using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace VoucherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        //This endpoint will called by Core API to take the Total AdyenSum and will return Total Adyen sum between two given dates
        [HttpPost("ZReport")]
        public dynamic GetAdyenSum(List<SettlementReportTableView> list)
        {
            List<dynamic> returnList = new List<dynamic>();
        
            Random random = new Random();

            foreach(SettlementReportTableView item in list)
            {
                Console.WriteLine(item.StartDate + " - " + item.EndDate);

                returnList.Add(new { StartDate = item.StartDate, EndDate = item.EndDate, TotalAdyenSum = Math.Round((random.NextDouble() * (85000 - 55000) + 55000), 2) });
            }

            return Ok(returnList);
        }
    }
}
