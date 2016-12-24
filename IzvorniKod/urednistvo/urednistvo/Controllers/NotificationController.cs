using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;

namespace urednistvo.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                return View(db.Notifications.ToList());
            }
        }

        // GET: Notification/Details/5
        public ActionResult Details(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var notification = db.Notifications.Where(d => d.NotificationId == id).ToList();
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
    }
}
