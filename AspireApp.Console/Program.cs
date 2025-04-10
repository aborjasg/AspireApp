// See https://aka.ms/new-console-template for more information

using AspireApp.ApiService.DataAccess;
using AspireApp.Libraries;
using AspireApp.Libraries.Models;
using AspireApp.ServiceDefaults.Shared;
using ConsoleApp;
using IdentityModel.OidcClient;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

/*
var appSettings = @"C:\Redlen\github\AspireApp\AspireApp.ApiService\PlotTemplates.json";
using (StreamReader r = new StreamReader(appSettings))
{
    string json = r.ReadToEnd();
    var output = JsonConvert.DeserializeObject <PictureTemplate[]>(json);
    //Console.WriteLine("Count = {0}", output!.PlotImages.Length);
    Console.WriteLine("JSON: OK");
}
Console.WriteLine("Hello, World!");
*/

Utils.EventLog("Information", "Start Program...");

/*
string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString.NpgsqlConnection")!;
var obj = new TableAccess<RunImage>(connectionString);

// Invoke API Plotter:
using (var httpClient = new HttpClient())
{
    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:7583/plotter"))
    {
        try
        {
            //var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("test@test.com:testPassword"));
            //request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

            request.Content = new StringContent("{\"Id\": 1}");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Utils.EventLog("Information", content);

        }
        catch (Exception ex)
        {            
            Utils.EventLog("Warning", ex.Message);
        }
    }
}


// Save record to DB:

//var record = new RunImage() { Metadata= "{\"Name\": \"Combined_NCP\"}", Image = content, CreatedDate = DateTime.Now };
//int result = obj.SaveRow(record);
//Utils.EventLog("Information", $"Row saved -> Result={result}");


// Read record from DB:

var id = 1;
var row = obj.GetRow(id);
Utils.EventLog("Information", $"Row Id={id} -> Got the record");
Utils.EventLog("Information", $"Row Id={id} -> Serialized: {JsonConvert.SerializeObject(row)}");
*/

//var result = FakeData.GetLineChartData();

// OptimizeServers [2025-04-08]
/*
var power = new List<int> { 1, 2, 3 }; // { 1, 4, 6, 3 }
var cost = new List<int> { 1, 4, 2 }; // { 1, 2, 4, 3 }
*/
//Console.WriteLine($"OptimizeServers: Result={Functions.OptimizeServers(power, cost)}");

// ParkingBill [2025-04-09]
/*
ParkingBill("10:00", "13:21") -> $17
ParkingBill("09:42", "11:42") -> $9
*/
//Console.WriteLine($"ParkingBill: Result={Functions.ParkingBill("09:42", "11:42")}");


// ParityDegree [2025-04-09]
/*
ParityDegree(24) -> 3
*/
Utils.EventLog("Information", $"ParityDegree: Result={Functions.ParityDegree(24)}");

Utils.EventLog("Information", "End of Program");
