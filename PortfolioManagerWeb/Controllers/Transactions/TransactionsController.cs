using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Interfaces;
using Newtonsoft.Json;
using PortfolioManager.DTO;
using PortfolioManager.DTO.DTOs.Transactions;

namespace PortfolioManagerWeb.Controllers.Transactions
{
    public class TransactionsController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> AccountTransactionSummary(int accountId)
        {         
            var client = PortfolioManagerHttpClient.GetClient();

            HttpResponseMessage response = await client.GetAsync(ApiPaths.AccountTransactions + "/" + accountId);
            //                    + "?fields=portfolioid,name,accounts");
            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var model = JsonConvert.DeserializeObject <AccountTransactionSummaryDto> (content);
                return View(model);
            }

            return Content("An error occurred");
        }
    }
}