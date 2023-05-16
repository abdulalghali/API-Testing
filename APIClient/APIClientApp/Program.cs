using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace APIClientApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            #region single postcode lookup
            // encapsulates the info we need to make an API call
            var restClient = new RestClient("https://api.postcodes.io/");
            // setup our request
            var restRequest = new RestRequest();
            // set up the method (by default, our RestRequest object method property is GET)
            restRequest.Method = Method.Get;
            // headers are Key value pairs
            restRequest.AddHeader("Content-Type", "application/json");
            // assign a postcode to a variable
            var postcode = "EC2Y 5AS";
            restRequest.Resource = $"postcodes/{postcode}";

            // execute request
            var singlePostCodeResponse = restClient.Execute(restRequest);
            //Console.WriteLine("Response content (json)");
            //Console.WriteLine(singlePostCodeResponse.Content);

            //Console.WriteLine("Response status (enum)");
            //Console.WriteLine((int)singlePostCodeResponse.StatusCode);
            //Console.WriteLine();

            var headers = singlePostCodeResponse.Headers;
            foreach (var header in headers)
            {
                //Console.WriteLine(header);
            }
            var responseDataHeader = headers.Where(p => p.Name == "Date").Select(h => h.Value.ToString()).FirstOrDefault();
            //Console.WriteLine(responseDataHeader);
            #endregion

            #region bulk postcode lookup
            var options = new RestClientOptions("https://api.postcodes.io")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/postcodes/", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var postcodes = new
            {
                Postcodes = new string[] { "OX49 5NU", "M32 0JG", "NE30 1DP" }
            };
            request.AddJsonBody(postcodes);
            //request.AddStringBody(body, DataFormat.Json);
            RestResponse bulkPostcodeResponse = await client.ExecuteAsync(request);
            //Console.WriteLine(bulkPostcodeResponse.Content);
            #endregion


            #region Converting results to JSON
            var singlePostcodeJsonResponse = JObject.Parse(singlePostCodeResponse.Content); //if the string can be parsed into a json object it will parse it, else it will throw an error
            //Console.WriteLine(singlePostcodeJsonResponse);
            //Console.WriteLine("Status Json Body");
            //Console.WriteLine(singlePostcodeJsonResponse["status"]);
            //Console.WriteLine(singlePostcodeJsonResponse["result"]["parliamentary_constituency"]);
            #endregion


            var bulkPostcodeJsonResponse = JObject.Parse(bulkPostcodeResponse.Content);
            //Console.WriteLine(bulkPostcodeJsonResponse);
            //Console.WriteLine(bulkPostcodeJsonResponse["result"][0]["result"]["parliamentary_constituency"]);


            #region Deserialise to POCO
            var singlePostcodesObjectResponse = JsonConvert.DeserializeObject<SinglePostcodeResponse>(singlePostCodeResponse.Content);
            //Console.WriteLine("SinglePostcodeResponse");
            //Console.WriteLine(singlePostcodesObjectResponse.result.codes.parliamentary_constituency);
            #endregion

            var bulkPostcodesObjectResponse = JsonConvert.DeserializeObject<BulkPostcodeResponse>(bulkPostcodeResponse.Content);
            //Console.WriteLine(bulkPostcodesObjectResponse.Status);
            //foreach (var item in bulkPostcodesObjectResponse.result)
            //{
            //    Console.WriteLine(item.query);
            //    Console.WriteLine(item.result.admin_district);
            //}

            #region Outcode
            // encapsulates the info we need to make an API call
            var outClient = new RestClient("https://api.postcodes.io/");
            // setup our request
            var outRequest = new RestRequest();
            // set up the method (by default, our RestRequest object method property is GET)
            outRequest.Method = Method.Get;
            // headers are Key value pairs
            outRequest.AddHeader("Content-Type", "application/json");
            // assign a postcode to a variable
            var outcode = "W13";
            outRequest.Resource = $"outcodes/{outcode}";

            // execute request
            var singleOutCodeResponse = outClient.Execute(outRequest);
            var singleOutcodeJsonResponse = JObject.Parse(singleOutCodeResponse.Content);
            var singleOutcodeJObjectResponse = JsonConvert.DeserializeObject<SingleOutcodeResponse>(singleOutCodeResponse.Content);

            Console.WriteLine(singleOutcodeJObjectResponse.result.parliamentary_constituency);

            Console.WriteLine($"Northings: {singleOutcodeJsonResponse["result"]["northings"]}");
            Console.WriteLine($"Eastings: {singleOutcodeJsonResponse["result"]["eastings"]}");
            Console.WriteLine($"Parliamentary Constituency: {singleOutcodeJsonResponse["result"]["parliamentary_constituency"]}");
            #endregion
        }
    }
}