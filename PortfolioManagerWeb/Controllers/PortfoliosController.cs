using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Interfaces;
using Newtonsoft.Json;
using PagedList;
using PortfolioManager.DTO;
using PortfolioManager.DTO.Requests;
using PortfolioManagerWeb.Helpers;
using PortfolioManagerWeb.Models;

namespace PortfolioManagerWeb.Controllers
{
    public class PortfoliosController : Controller  //Ensure this is NOT APIController
    {
        public async Task<ActionResult> Index(int? page = 1)
        {

            var client = PortfolioManagerHttpClient.GetClient();

            var model = new PortfoliosViewModel();

            //HttpResponseMessage egsResponse = await client.GetAsync("api/expensegroupstatusses");

            //if (egsResponse.IsSuccessStatusCode)
            //{
            //    string egsContent = await egsResponse.Content.ReadAsStringAsync();
            //    var lstExpenseGroupStatusses = JsonConvert.DeserializeObject<IEnumerable<ExpenseGroupStatus>>(egsContent);
            //    model.ExpenseGroupStatusses = lstExpenseGroupStatusses;
            //}
            //else
            //{
            //    return System.Web.UI.WebControls.Content("An error occurred.");
            //}

            //string userId = (this.User.Identity as ClaimsIdentity).FindFirst("unique_user_key").Value;

            //HttpResponseMessage response = await client.GetAsync("api/expensegroups?sort=expensegroupstatusid"
            //    + ",title&page=" + page + "&pagesize=5&userid=" + userId);

            HttpResponseMessage response = await client.GetAsync(ApiPaths.Portfolios + "?page=" + page + "&pagesize=5");
                //"?sort=expensegroupstatusid"+ ",title&page=" + page + "&pagesize=5");


            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                // get the paging info from the header
                var pagingInfo = HeaderParser.FindAndParsePagingInfo(response.Headers);

                var portfolios = JsonConvert.DeserializeObject<IEnumerable<PortfolioDto>>(content);

                var pagedExpenseGroupsList = new StaticPagedList<PortfolioDto>(portfolios, pagingInfo.CurrentPage,
                    pagingInfo.PageSize, pagingInfo.TotalCount);

                model.Portfolios = pagedExpenseGroupsList;
                model.PagingInfo = pagingInfo;
            }
            else
            {
                return Content("An error occurred.");
            }


            return View(model);

        }

        // GET: ExpenseGroups/Details/5
        public async Task<ActionResult> Details(int portfolioId)
        {
            var client = PortfolioManagerHttpClient.GetClient();

            HttpResponseMessage response = await client.GetAsync(ApiPaths.Portfolios + "/" + portfolioId
                                + "?fields=portfolioid,name,accounts");
            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var model = JsonConvert.DeserializeObject<PortfolioDto>(content);
                return View(model);
            }

            return Content("An error occurred");
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(PortfolioRequest portfolio)
        {

            try
            {
                var client = PortfolioManagerHttpClient.GetClient();


                //var claimsIdentity = this.User.Identity as ClaimsIdentity;
                //var userId = claimsIdentity.FindFirst("unique_user_key").Value;

                // an expensegroup is created with status "Open", for the current user
                //expenseGroup.ExpenseGroupStatusId = 1;
                //expenseGroup.UserId = userId;

                var serializedItemToCreate = JsonConvert.SerializeObject(portfolio);

                var response = await client.PostAsync(ApiPaths.Portfolios,
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
