using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Interfaces;
using Newtonsoft.Json;
using PortfolioManager.DTO.DTOs.PriceUpdates;
using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManagerWeb.Models;

namespace PortfolioManagerWeb.Controllers.PriceUpdate
{
    public class PriceUpdateController : Controller
    {
        
        public ActionResult EditPrice(int? investmentId)
        {
            var y = new InvestmentPriceSummaryDto
            {
                InvestmentId = 1,
                InvestmentName = "Happy",
                LatestBuyPrice = (decimal)1.20,
                LatestSellPrice = (decimal)1.09,
                LatestBuyPriceDate = DateTime.Today,
                LatestSellPriceDate = DateTime.Today.AddDays(-4)
            };

            var z = new InvestmentPriceSummaryDecorator()
            {
                InvestmentPriceSummary = y
            };

            return View(z);
        }
        
        [HttpPost]
        public async Task<ActionResult> EditPrice(InvestmentPriceSummaryDecorator investmentPriceSummaryDecorator)
        {
            try
            {
                var response = await ProcessSinglePriceUpdate(investmentPriceSummaryDecorator);

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

        private static async Task<HttpResponseMessage> ProcessSinglePriceUpdate(InvestmentPriceSummaryDecorator investmentPriceSummary)
        {
            var client = PortfolioManagerHttpClient.GetClient();

            var priceUpdateRequest = new PriceHistoryRequest();
            priceUpdateRequest.InvestmentId = investmentPriceSummary.InvestmentPriceSummary.InvestmentId;
            priceUpdateRequest.SellPrice = string.IsNullOrWhiteSpace(investmentPriceSummary.NewSellPrice)
                ? (decimal?) null
                : Decimal.Parse(investmentPriceSummary.NewSellPrice);
            priceUpdateRequest.BuyPrice = string.IsNullOrWhiteSpace(investmentPriceSummary.NewBuyPrice)
                ? (decimal?) null
                : Decimal.Parse(investmentPriceSummary.NewBuyPrice);
            priceUpdateRequest.ValuationDate = investmentPriceSummary.ValuationDate;


            var serializedItemToCreate = JsonConvert.SerializeObject(priceUpdateRequest);

            var response = await client.PostAsync(ApiPaths.InvestmentSinglePriceUpdate,
                new StringContent(serializedItemToCreate,
                    System.Text.Encoding.Unicode, "application/json"));
            return response;
        }
    }
}
