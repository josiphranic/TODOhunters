using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;

namespace urednistvo.Controllers
{
    public class TextController : Controller
    {
        // GET: Text
        public ActionResult Index()
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                return View(db.Texts.ToList());
            }
        }

        // GET: Text/Details/5
        public ActionResult Details(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var text = db.Texts.Single(d => d.TextId == id);
                return View(text);
            }
        }

        // GET: Text/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Text/Create
        [HttpPost]
        public ActionResult Create(Text text)
        {
            if (ModelState.IsValid)
            {
                using (UrednistvoDatabase db = new UrednistvoDatabase())
                {
                    text.TextStatus = (int)TextStatus.SENT;
                    text.WebPublishable = false;
                    text.EditionPublishable = false;
                    text.FinalSectionId = -1;
                    text.UserId = Int32.Parse((String)(Session["UserID"]));
                    text.Time = DateTime.Now;

                    db.Texts.Add(text);
                    db.SaveChanges();
                }
                ModelState.Clear();
                //ViewBag.Message = "Text \"" + text.Title + " made at " + notification.Time + ".";
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Text/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Text/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Text text)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Text t = db.Texts.Find(id);
                text.Title = t.Title;
                text.Time = DateTime.Now;

                db.Texts.Remove(t);
                db.Texts.Add(text);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        // GET: Text/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Text/Delete/5
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
