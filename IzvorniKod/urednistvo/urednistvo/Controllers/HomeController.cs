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
                int currentId = (Session["UserId"] == null) ? 0 : Int32.Parse((String)Session["UserId"]);
                var notifs = new List<Notification>();
                foreach(var notif in db.Notifications.ToList())
                {
                    if (notif.Users == null || notif.Users.Count == 0)
                    {
                        notifs.Add(notif);
                        continue;
                    }
                    foreach (User u in notif.Users)
                    {
                        if (u.UserId == currentId)
                        {
                            notifs.Add(notif);
                        }
                    }
                }
                var texts = db.Texts.Where(x => x.WebPublishable).ToList();
                if (currentId != 0)
                {
                    texts.Concat(db.Texts.Where(x => x.EditionPublishable).ToList());
                }
                Tuple<IEnumerable<Text>, IEnumerable<Notification>> tuple = new Tuple<IEnumerable<Text>, IEnumerable<Notification>>(texts,notifs);
                return View(tuple);
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