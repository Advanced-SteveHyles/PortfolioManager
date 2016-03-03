using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PortfolioManagerWeb.Controllers
{
    public class PortfolioManagerHttpClient
    {
        public static HttpClient GetClient(string requestedVersion = null)
        {

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(PortfolioManagerConstants.PortfolioManagerApi);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            if (requestedVersion != null)
            {
                // through a custom request header
                //client.DefaultRequestHeaders.Add("api-version", requestedVersion);

                // through content negotiation
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.expensetrackerapi.v"
                                                        + requestedVersion + "+json"));
            }


            return client;
        }
    }

    public class PortfolioManagerConstants
    {
        public static string PortfolioManagerApi => "http://localhost:59274/";
    }
}