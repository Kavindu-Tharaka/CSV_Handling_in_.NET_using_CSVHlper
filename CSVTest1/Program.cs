using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSVTest1
{
    public class Program
    {
        //This console project will show how to calculate Total Payout when Journal type is equal to MerchantPayout
        static async Task Main(string[] args)
        {
            StreamReader streamReader = await DownloadSettlementReportCSVToMemory();

            decimal totalPayout = CalculateTotalPayout(streamReader);

            Console.WriteLine(totalPayout);  // for this file it will print 0 since it does not have sufficient data in the csv file.

        }

        private static async Task<StreamReader> DownloadSettlementReportCSVToMemory()
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

                    return new StreamReader(await responseContent.ReadAsStreamAsync());
                }
            }
        }

        private static decimal CalculateTotalPayout(StreamReader streamReader)
        {
            decimal totalPayout = 0;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,  //This will not throw error if a cell has no value (empty string)
            };

            using (var reader = streamReader)
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<SettlementReportMap>();

                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = csv.GetRecord<SettlementReport>();

                    if (record != null && record.Type == "MerchantPayout")
                    {
                        var netCreditNC = record.NetCreditNC != "" ? Decimal.Parse(record.NetCreditNC) : 0;  //This will handle if a cell has no value

                        totalPayout += netCreditNC;

                        Console.WriteLine(record.PaymentMethod);
                    }
                }
            }
            return totalPayout;
        }
    }
}
