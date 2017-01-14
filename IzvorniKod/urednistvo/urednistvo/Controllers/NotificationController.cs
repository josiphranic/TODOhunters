using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView.Textual;
using urednistvo.ModelsView.Utilities;

namespace urednistvo.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            if ((String)Session["Role"] == null)
            {
                TempData["Message"] = "Nemate ovlasti pristupiti obavijestima.";
                return RedirectToAction("Index", "Text");
            }

            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                int currentId = (Session["UserId"] == null) ? 0 : Int32.Parse((String)Session["UserId"]);
                List<NotificationView> notificationViews = new List<NotificationView>();

                foreach(Notification n in db.Notifications.ToList())
                {
                    if (n.Users == null || n.Users.Count == 0)
                    {
                        notificationViews.Add(createNotificationView(n, 0));
                        continue;
                    }
                    else if(currentId != 0)
                    {
                        foreach (User u in n.Users)
                        {
                            if (u.UserId == currentId)
                            {
                                notificationViews.Add(createNotificationView(n, currentId));
                                break;
                            }
                        }
                    }
                }

                if(notificationViews.Count == 0)
                {
                    TempData["Message"] = "Nema obavijesti za prikazati";
                    return RedirectToAction("Index", "Home");
                }
                return View(notificationViews);
            }
        }

        private NotificationView createNotificationView(Notification n, int UserId)
        {
            NotificationView nView = new NotificationView();

            nView.NotificationId = n.NotificationId;
            nView.Title = n.Title;
            nView.Time = n.Time;
            nView.Content = n.Content;
            nView.UserId = UserId;
            nView.ForUser = n.Users.Count == 0 ? "Javno" : n.Users.ElementAt(0).UserName;

            return nView;
        }

        // GET: Notification/Details/5
        public ActionResult Details(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var notification = db.Notifications.Single(d => d.NotificationId == id);
                return View(notification);
            }
        }

        // GET: Notification/Create
        public ActionResult Create()
        {
            // DODATI ZABRANU PRISTUPA
            return View();
        }

        // POST: Notification/Create
        [HttpPost]
        public ActionResult Create(Notification notification)
        {
            notification.Time = DateTime.Now;

            if (ModelState.IsValid)
            {
                using (UrednistvoDatabase db = new UrednistvoDatabase())
                {
                    db.Notifications.Add(notification);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = "Notification \"" + notification.Title + " made at " + notification.Time + ".";
                return RedirectToAction("Index");
            }
            return View();
        }

        public static void createNotificationForEditor(Text text, string message)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Notification notification = new Notification();

                int roleValue = db.Roles.Single(r => r.RoleName == RoleNames.EDITOR).Value;

                notification.Title = "Obavijest o tekstu: \" " + text.Title + "\"";
                notification.Content = message;
                notification.Users.Add(db.Users.Single(u => u.UserId == text.UserId));
                notification.Time = DateTime.Now;

                db.Notifications.Add(notification);
                db.SaveChanges();
            }
        }

        public static void createNotificationForLector(Text text)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Notification notification = new Notification();

                int roleValue = db.Roles.Single(r => r.RoleName == RoleNames.LECTOR).Value;

                notification.Title = "Novi tekst za lektoriranje";
                notification.Content = "Tekst \" " + text.Title + "\" je spreman za vase lektoriranje.";
                notification.Users = db.Users.Where(u => u.Role == roleValue).ToList();
                notification.Time = DateTime.Now;

                db.Notifications.Add(notification);
                db.SaveChanges();
            }
        }

        public static void createNotificationForGraphicEditor(Text text)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Notification notification = new Notification();

                int roleValue = db.Roles.Single(r => r.RoleName == RoleNames.GRAPHIC_EDITOR).Value;

                notification.Title = "Novi tekst za grafičko uređivanje";
                notification.Content = "Tekst \" " + text.Title + "\" je spreman za vase grafičko uređivanje.";
                notification.Users = db.Users.Where(u => u.Role == roleValue).ToList();
                notification.Time = DateTime.Now;

                db.Notifications.Add(notification);
                db.SaveChanges();
            }
        }

        public static void createNotificationForCorrector(Text text)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Notification notification = new Notification();

                int roleValue = db.Roles.Single(r => r.RoleName == RoleNames.CORRECTOR).Value;

                notification.Title = "Novi tekst za korekciju";
                notification.Content = "Tekst \" " + text.Title + "\" je spreman za vašu korekciju.";
                notification.Users = db.Users.Where(u => u.Role == roleValue).ToList();
                notification.Time = DateTime.Now;

                db.Notifications.Add(notification);
                db.SaveChanges();
            }
        }

        public static void createNotificationForAuthor(Text text, string message)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Notification notification = new Notification();

                notification.Title = "Obavijest o vašem tekstu \"" + text.Title + "\"";
                notification.Content = message;
                notification.Users = db.Users.Where(u => u.UserId == text.UserId).ToList();
                notification.Time = DateTime.Now;

                db.Notifications.Add(notification);
                db.SaveChanges();
            }
        }       
    }
}
