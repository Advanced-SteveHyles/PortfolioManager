using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortfolioManagerWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public ActionResult Portfolios()
        {
            ViewBag.Message = "Your portfolio page.";
            
            return View();
        }

        public ActionResult Investments()
        {
            ViewBag.Message = "Investments page.";

            return View();
        }
    }
}