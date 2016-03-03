using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Interfaces;
using Newtonsoft.Json;
using PortfolioManager.DTO.Transactions;

namespace PortfolioManagerWeb.Controllers.Investments
{
    public class BuyInvestmentController : Controller
    {
        public ActionResult Buy(int? investmentMapId)
        {
            var investmentBuyRequest = new InvestmentBuyRequest()
            {
                PurchaseDate = DateTime.Today,
                SettlementDate    = DateTime.Today,
            };

            return View(investmentBuyRequest);
        }

        [HttpPost]
        public async Task<ActionResult> Buy(InvestmentBuyRequest buy)
        {
            try
            {                
                var response = await ProcessBuyTransaction(buy);

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

        private static async Task<HttpResponseMessage> ProcessBuyTransaction(InvestmentBuyRequest buy)
        {
            var client = PortfolioManagerHttpClient.GetClient();

            var serializedItemToCreate = JsonConvert.SerializeObject(buy);

            var response = await client.PostAsync(ApiPaths.BuyTransaction,
                new StringContent(serializedItemToCreate,
                    System.Text.Encoding.Unicode, "application/json"));
            return response;
        }
    }
}