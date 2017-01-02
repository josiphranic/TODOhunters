using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace urednistvo.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            if((String)Session["Role"] == "Glavni urednik" || (String)Session["Role"] == "Član uredničkog vijeća") {
                TempData["Message"] = "Samo glavni urednik i članovi uredničko vijeća imaju pristup statistikama.";
            }
            return View();
        }


    }
}