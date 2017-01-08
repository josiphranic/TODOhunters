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
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                int currentId = (Session["UserId"] == null) ? 0 : Int32.Parse((String)Session["UserId"]);
                List<NotificationView> notificationViews = new List<NotificationView>();

                foreach(Notification n in db.Notifications.ToList())
                {
                    if(n.Users == null || n.Users.Count == 0)
                    {
                        notificationViews.Add(createNotificationView(n, 0));
                        continue;
                    }
                    foreach(User u in n.Users)
                    {
                        if(u.UserId == currentId)
                        {
                            notificationViews.Add(createNotificationView(n, currentId));
                            break;
                        }
                    }
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
            nView.ForUser = n.Users.Count == 0 ? "Public" : n.Users.ElementAt(0).UserName;

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

        // GET: Notification/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Notification/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Notification notification)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Notification n = db.Notifications.Find(id);
                notification.Title = n.Title;
                notification.Time = DateTime.Now;

                db.Notifications.Remove(n);
                db.Notifications.Add(notification);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        // GET: Notification/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Notification/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public static void createNotification(Text text, string message)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Notification notification = new Notification();

                notification.Title = "Obavijest o vasem tekstu: \" " + text.Title + "\"";
                notification.Content = message;
                notification.Users.Add(db.Users.Single(u => u.UserId == text.UserId));
                //PROVJERITI REDAK IZNAD JEL OK
                notification.Time = DateTime.Now;

                db.Notifications.Add(notification);
                db.SaveChanges();
            }
        }

        public static void createNotification(Role Role, Text text, string message)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Notification notification = new Notification();

                notification.Title = "Tekst \" " + text.Title + "\" je spreman za vase lektoriranje.";
                notification.Content = message;
                notification.Users.Add(db.Users.Single(u => u.Role == (int)Role));
                notification.Time = DateTime.Now;

                db.Notifications.Add(notification);
                db.SaveChanges();
            }
        }
    }
}
