using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Interfaces;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;
using PagedList;
using PortfolioManager.DTO;
using PortfolioManager.DTO.DTOs.PriceUpdates;
using PortfolioManagerWeb.Helpers;
using PortfolioManagerWeb.Models;

namespace PortfolioManagerWeb.Controllers.Investments
{
    public class InvestmentsController : Controller
    {
        public async Task<ActionResult> Index(int? page = 1)
        {
            var client = PortfolioManagerHttpClient.GetClient();
            var model = new InvestmentsViewModel();

            HttpResponseMessage response = await client.GetAsync(ApiPaths.Investments +"?page=" + page + "&pagesize=5");
            //"?sort=expensegroupstatusid"+ ",title&page=" + page + "&pagesize=5");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var pagingInfo = HeaderParser.FindAndParsePagingInfo(response.Headers);

                var investments = JsonConvert.DeserializeObject<IEnumerable<InvestmentDto>>(content);

                var pagedInvestmentsList = new StaticPagedList<InvestmentDto>(investments, pagingInfo.CurrentPage,
                pagingInfo.PageSize, pagingInfo.TotalCount);

                model.Investments = pagedInvestmentsList;
                model.PagingInfo = pagingInfo;
            }
            else
            {
                return Content("An error occurred.");
            }

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(InvestmentRequest investmentRequest)
        {

            try
            {
                var client = PortfolioManagerHttpClient.GetClient();


                //var claimsIdentity = this.User.Identity as ClaimsIdentity;
                //var userId = claimsIdentity.FindFirst("unique_user_key").Value;

                // an expensegroup is created with status "Open", for the current user
                //expenseGroup.ExpenseGroupStatusId = 1;
                //expenseGroup.UserId = userId;

                var serializedItemToCreate = JsonConvert.SerializeObject(investmentRequest);

                var response = await client.PostAsync(ApiPaths.Investments,
                  new StringContent(serializedItemToCreate,
                  System.Text.Encoding.Unicode, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
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
