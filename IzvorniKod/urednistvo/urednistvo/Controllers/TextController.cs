using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;
using urednistvo.ModelsView.Textual;
using urednistvo.ModelsView.Utilities;

namespace urednistvo.Controllers
{
    public class TextController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        public TextView getTextView(Text text)
        {
            TextView textView = new TextView();

            textView.TextId = text.TextId;
            textView.Title = text.Title;
            textView.Subtitle = text.Subtitle;
            textView.Username = db.Users.Single(u => u.UserId == text.UserId).UserName;
            textView.UserId = text.UserId;
            textView.Time = text.Time;
            textView.Content = text.Content;
            textView.TextStatus = TextStatusNameGetter.getName(text.TextStatus);
            textView.WebPublishable = text.WebPublishable.ToString();
            textView.EditionPublishable = text.EditionPublishable.ToString();

            if (db.Sections.Count() != 0)
            {
                Section section = db.Sections.Single(s => s.SectionId == text.FinalSectionId);
                textView.FinalSection = (section == null) ? "-" : section.Title;
                section = db.Sections.Single(s => s.SectionId == text.WantedSectionByAuthorId);
                textView.WantedSectionByAuthor = (section == null) ? "-" : section.Title;
            }
            else
            {
                textView.FinalSection = "-";
                textView.WantedSectionByAuthor = "-";
            }

            return textView;
        } 

        // GET: Text
        public ActionResult Index()
        {
            List<Text> list = db.Texts.ToList();
            List<TextView> listView = new List<TextView>();

            foreach(Text t in list.ToList())
            {
                listView.Add(getTextView(t));
            }
            return View(listView);
        }

        // GET: Text/Details/5
        public ActionResult Details(int id)
        {
            return View(getTextView(db.Texts.Single(u => u.TextId == id)));
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
                var query = from ord in db.Texts
                            where ord.TextId == id
                            select ord;

                foreach(Text t in query)
                {
                    t.EditionPublishable = text.EditionPublishable;
                    t.WebPublishable = text.WebPublishable;
                    //t.TextStatus = TextStatus.ACCEPTED
                    t.Suggestions = text.Suggestions;
                    t.FinalSectionId = text.FinalSectionId;
                    t.FinalSection = text.FinalSection;
                }

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
