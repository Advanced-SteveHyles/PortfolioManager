using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Interfaces;
using Newtonsoft.Json;
using PortfolioManager.DTO.Requests.Transactions;

namespace PortfolioManagerWeb.Controllers
{
    public class AccountTransactionController : Controller
    {

        public ActionResult DepositFunds(int? accountId)
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> DepositFunds(DepositTransactionRequest depositTransactionRequest)
        {
            try
            {
                var response = await CreateDeposit(depositTransactionRequest);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Accounts", new { accountId = depositTransactionRequest.AccountId });
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

        private static async Task<HttpResponseMessage> CreateDeposit(DepositTransactionRequest depositTransactionRequest)
        {
            var client = PortfolioManagerHttpClient.GetClient();


            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst("unique_user_key").Value;

            // an expensegroup is created with status "Open", for the current user
            //expenseGroup.ExpenseGroupStatusId = 1;
            //expenseGroup.UserId = userId;

            var serializedItemToCreate = JsonConvert.SerializeObject(depositTransactionRequest);

            var response = await client.PostAsync(ApiPaths.CashDeposit,
                new StringContent(serializedItemToCreate,
                    System.Text.Encoding.Unicode, "application/json"));
            return response;
        }


        public ActionResult WithdrawFunds(int? accountId)
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> WithdrawFunds(WithdrawalTransactionRequest withdrawalTransactionRequest)
        {
            try
            {
                var response = await CreateWithdrawal(withdrawalTransactionRequest);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Accounts", new { accountId = withdrawalTransactionRequest.AccountId });
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


        private static async Task<HttpResponseMessage> CreateWithdrawal(WithdrawalTransactionRequest withdrawalTransactionRequest)
        {
            var client = PortfolioManagerHttpClient.GetClient();


            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst("unique_user_key").Value;

            // an expensegroup is created with status "Open", for the current user
            //expenseGroup.ExpenseGroupStatusId = 1;
            //expenseGroup.UserId = userId;

            var serializedItemToCreate = JsonConvert.SerializeObject(withdrawalTransactionRequest);

            var response = await client.PostAsync(ApiPaths.CashWithdrawal,
                new StringContent(serializedItemToCreate,
                    System.Text.Encoding.Unicode, "application/json"));
            return response;
        }
    }
}