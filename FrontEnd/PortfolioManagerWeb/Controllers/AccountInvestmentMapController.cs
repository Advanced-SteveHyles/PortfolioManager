using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Interfaces;
using Newtonsoft.Json;
using PortfolioManager.DTO;
using PortfolioManager.DTO.DTOs;
using PortfolioManager.DTO.Requests;
using PortfolioManagerWeb.Models;
using AccountInvestmentMapDto = PortfolioManager.DTO.DTOs.AccountInvestmentMapDto;

namespace PortfolioManagerWeb.Controllers
{
    public class AccountInvestmentMapController : Controller
    {
        public async Task<ActionResult> LinkInvestment(int accountId)
        {
            var client = PortfolioManagerHttpClient.GetClient();

            HttpResponseMessage response = await client.GetAsync(ApiPaths.InvestmentMap + "/" + accountId);

            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var accountInvestmentMap = JsonConvert.DeserializeObject<AccountInfoWithAllInvestmentDto>(content);
                return View(accountInvestmentMap);
            }

            return Content("An error occurred");
        }


        public async Task<ActionResult> LinkAccountToInvestment(int accountId, int investmentId)
        {
            var linkRequest = new AccountInvestmentMapRequest {AccountId = accountId, InvestmentId = investmentId};

            try
            {
                var client = PortfolioManagerHttpClient.GetClient();
                
                //var claimsIdentity = this.User.Identity as ClaimsIdentity;
                //var userId = claimsIdentity.FindFirst("unique_user_key").Value;

                // an expensegroup is created with status "Open", for the current user
                //expenseGroup.ExpenseGroupStatusId = 1;
                //expenseGroup.UserId = userId;

                var serializedItemToCreate = JsonConvert.SerializeObject(linkRequest);

                var response = await client.PostAsync(ApiPaths.InvestmentMap,
                    new StringContent(serializedItemToCreate,
                        System.Text.Encoding.Unicode, "application/json"));
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LinkInvestment", "AccountInvestmentMap", new
                    {
                        accountId = linkRequest.AccountId
                    });
                }
                else
                {
                    return Content("An error occurred");
                }
            }
            catch
            {
                return Content("An error occurred.");
            }
        }
        
    }
}