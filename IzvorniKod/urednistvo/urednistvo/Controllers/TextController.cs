using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;
using urednistvo.ModelsView.Textual;

namespace urednistvo.Controllers
{
    public class TextController : Controller
    {
        // GET: Text
        public ActionResult Index()
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                List<Text> list = db.Texts.ToList();
                List<TextIndexViewModel> listView = new List<TextIndexViewModel>();

                foreach(Text t in list.ToList())
                {
                    TextIndexViewModel tivm = new TextIndexViewModel();

                    tivm.TextID = t.TextId;
                    tivm.Title = t.Title;
                    tivm.Subtitle = t.Subtitle;
                    tivm.Username = db.Users.Single(u => u.UserId == t.UserId).UserName;
                    tivm.UserID = t.UserId;

                    if(t.FinalSection == null)
                    {
                        tivm.FinalSection = "-";
                    }
                    else
                    {
                        tivm.FinalSection = t.FinalSection.ToString();
                    }
                    tivm.Time = t.Time;

                    listView.Add(tivm);
                }
                return View(listView);
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

                    text.UserId = Int32.Parse((String)(Session["UserID"]));;

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

        // GET: Text/EditEditor/5
        public ActionResult EditEditor(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var text = db.Texts.Single(d => d.TextId == id);
                return View(text);
            }
        }

        // POST: Text/EditEditor/5
        [HttpPost]
        public ActionResult EditEditor(int id, Text text)
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

        // GET: Text/EditMember/5
        public ActionResult EditMember(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var text = db.Texts.Single(d => d.TextId == id);
                return View(text);
            }
        }

        // POST: Text/EditMember/5
        [HttpPost]
        public ActionResult EditMember(int id, Text text)
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
