using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdyenController : ControllerBase
    {
        //This endpoint will call Voucher Service API and get the Total AdyenSum
        //But this operation should be a method get invoked in the create command handler of the Settlement Report 
        [HttpGet("TotalAdyenSum")]
        public async Task<IActionResult> GetAdyenSettlementReport()
        {
            var dateRanges = new List<SettlementReportTableView>
            {
                new SettlementReportTableView(new DateTime(2020, 8, 1), new DateTime(2020, 8, 15)),
                new SettlementReportTableView(new DateTime(2020, 9, 1), new DateTime(2020, 9, 15)),
                new SettlementReportTableView(new DateTime(2020, 10, 1), new DateTime(2020, 10, 15)),
            };

            var json = JsonConvert.SerializeObject(dateRanges);
            var data = System.Net.Http.Json.JsonContent.Create(dateRanges, null, null);

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };            
            HttpClient client = new HttpClient(clientHandler);

            var url = "http://localhost:29874/api/Voucher/ZReport";
            var response = await client.PostAsync(url, data);
            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);
        }

        //This endpoint will generate Table view (with columns a to f) of the settlement reports
        [HttpGet("SettlementReportTableView")]
        public ActionResult GetSettlementReportTableView()
        {
            //Consider this list as a list we retrieved from the DB
            var records = new List<SettlementReportTableView>
            {
                new SettlementReportTableView { ReportName = "Rep,ort 1", BatchNumber = 1, StartDate = new DateTime(2020, 2, 2), EndDate = new DateTime(2020, 3, 2), TotalPayout = 3500.50, TotalAdyenSum = 3800  },
                new SettlementReportTableView { ReportName = "Report's 2", BatchNumber = 1, StartDate = new DateTime(2020, 2, 2), EndDate = new DateTime(2020, 3, 2), TotalPayout = 3500.50, TotalAdyenSum = 3800  },
                new SettlementReportTableView { ReportName = "R.eport!@#$%^&*() 3", BatchNumber = 1, StartDate = new DateTime(2020, 2, 2), EndDate = new DateTime(2020, 3, 2), TotalPayout = 3500.50, TotalAdyenSum = 3800  },

            };

            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ",", HasHeaderRecord = true };

            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.Context.RegisterClassMap<SettlementReportTableViewMap>();

                csv.WriteHeader<SettlementReportTableView>();

                csv.NextRecord();
                foreach (var record in records)
                {
                    csv.WriteRecord(record);
                    csv.NextRecord();
                }

                writer.Flush();

                var result = Encoding.UTF8.GetString(mem.ToArray());

                byte[] bytes = Encoding.ASCII.GetBytes(result);

                return new FileContentResult(bytes, "text/csv")
                {
                    FileDownloadName = "Sample_name_table_view"    //Have to customize this file name accordingly
                };
            }
        }

        //This endpoint will download a single Settelement report from Adyen Test Account
        //NOTE: almost 90% of below implementation is in the DownloadSettlementReportCSVToMemory() method of Program.cs file of CSVTest1 project
        [HttpGet("SingleAdyenSettlementReport")]
        public async Task<IActionResult> DownloadSingleAdyenSettlementReport()
        {
            var handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; //To bypass invalid SSL certificate

            //handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;    //older method to bypass invaliud SSL

            //Below 2 strings can take using the REPORT_AVAILABLE notification from Adyen or can get from the DB if we save these details
            var pspReference = "settlement_detail_report_batch_1.csv";  //the file name of the report.
            var reasonURL = @"https:\/\/ca-test.adyen.com\/reports\/download\/MerchantAccount\/TestCompany086ECOM\/settlement_detail_report_batch_1.csv"; //Download URL

            var AccountType = reasonURL.Split(@"\/")[5];
            var AccountCode = reasonURL.Split(@"\/")[6];

            var downloadURL = $"https://ca-test.adyen.com/reports/download/{AccountType}/{AccountCode}/{pspReference}";

            var userName = "report_229384@Company.TestCompany086";  //username of the reporting API credential in Adyen test account
            var password = "67g:ZVTTXsyudAR%pm;5[6?)E"; //Basic auth password of the reporting API credential in Adyen test account

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), downloadURL))
                {
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    HttpContent responseContent = response.Content;

                    byte[] bytes =  await responseContent.ReadAsByteArrayAsync();

                    return new FileContentResult(bytes, "text/csv")
                    {
                        FileDownloadName = pspReference
                    };
                }
            }
        }
    }
}
