using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using PortfolioManager.DTO;
using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManagerWeb.Models;

namespace PortfolioManagerWeb.Controllers
{
    public class AccountsController : Controller
    {
        public ActionResult Create(int? portfolioId)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(AccountRequest account)
        {
            try
            {
                var response = await CreateAccount(account);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Portfolios", new { portfolioId = account.PortfolioId });
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

        private static async Task<HttpResponseMessage> CreateAccount(AccountRequest account)
        {
            var client = PortfolioManagerHttpClient.GetClient();


            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst("unique_user_key").Value;

            // an expensegroup is created with status "Open", for the current user
            //expenseGroup.ExpenseGroupStatusId = 1;
            //expenseGroup.UserId = userId;

            var serializedItemToCreate = JsonConvert.SerializeObject(account);

            var response = await client.PostAsync("api/accounts",
                new StringContent(serializedItemToCreate,
                    System.Text.Encoding.Unicode, "application/json"));
            return response;
        }


        public async Task<ActionResult> Edit(int accountId)
        {
            return null;
        }

        public async Task<ActionResult> Details(int accountId)
        {
            var client = PortfolioManagerHttpClient.GetClient();

            HttpResponseMessage response = await client.GetAsync("api/accounts/" + accountId
                                + "?fields=accountid,portfolioid,name,investments");

            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var model = JsonConvert.DeserializeObject<AccountDto>(content);
                return View(model);
            }

            return Content("An error occurred");
        }

        
    }
}