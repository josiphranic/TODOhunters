using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;

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
            return View();
        }

        public ActionResult EditorialCouncil()
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                int RoleEditor = db.Roles.Single(r => r.RoleName == "Glavni urednik").Value;
                int RoleMember = db.Roles.Single(r => r.RoleName == "Član uredničkog vijeća").Value;

                List<UserView> uViews = new List<UserView>();

                foreach (User user in db.Users.Where(u => u.Role == RoleEditor || u.Role == RoleMember))
                {
                    uViews.Add(UserController.createUserView(user));
                }

                return View(uViews);
            }
        }
    }
}