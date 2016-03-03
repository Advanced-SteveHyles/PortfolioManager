using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortfolioManagerWeb.Controllers.Investments
{
    public class InvestmentsMapController : Controller
    {
        private readonly CorporateActionController _corporateActionController;

        public InvestmentsMapController()
        {
            _corporateActionController = new CorporateActionController(this);
        }

        public ActionResult Sell(int id)
        {
            return View();
        }


        public async Task<ActionResult> Dividend(int id)
        {
            return View();
        }


        public async Task<ActionResult> Resolves()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ActionResult> Resolve(object id)
        {
            throw new System.NotImplementedException();
        }
    }
}