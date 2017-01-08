using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;

namespace urednistvo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                // ovdje u svemu sto se salje na pocetni view filtrirati sto se tice logiranog korisnika
                // prenijeti i popis tekstova!
                return View(db.Notifications.ToList());
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}