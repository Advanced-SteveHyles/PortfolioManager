using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Interfaces;
using Newtonsoft.Json;
using PortfolioManager.DTO.Requests.Transactions;

namespace PortfolioManagerWeb.Controllers.Investments
{
    public class CorporateActionController : Controller
    {
        private InvestmentsMapController _investmentsMapController;

        public CorporateActionController(InvestmentsMapController investmentsMapController)
        {
            _investmentsMapController = investmentsMapController;
        }

        public ActionResult CorporateAction(int? investmentMapId)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CorporateAction(InvestmentCorporateActionRequest request)
        {
            try
            {
                var response = await ProcessCorporateActionTransaction(request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Accounts", new { id = 1 });
                }
                else
                {
                    return Content("An error occurred");
                }
            }
            catch
            {
                return Content("An error occurred");
            }
        }

        private static async Task<HttpResponseMessage> ProcessCorporateActionTransaction(InvestmentCorporateActionRequest    corporateActionRequest)
        {
            var client = PortfolioManagerHttpClient.GetClient();

            var serializedItemToCreate = JsonConvert.SerializeObject(corporateActionRequest);

            var response = await client.PostAsync(ApiPaths.CorporateAction,
                new StringContent(serializedItemToCreate,
                    System.Text.Encoding.Unicode, "application/json"));
            return response;
        }
    }
}