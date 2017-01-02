using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;
using urednistvo.ModelsView.Textual;
using urednistvo.ModelsView.Utilities;
using static System.Net.WebRequestMethods;

namespace urednistvo.Controllers
{
    public class TextController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        public static TextView getTextView(Text text)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
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
                textView.FinalSectionId = text.FinalSectionId;
                textView.WantedSectionByAuthorId = text.WantedSectionByAuthorId;

                Section section = db.Sections.Find(text.FinalSectionId);
                textView.FinalSection = (section == null) ? "-" : section.Title;

                if (text.WantedSectionByAuthorId != null)
                {
                    section = db.Sections.Find(text.WantedSectionByAuthorId);
                    textView.WantedSectionByAuthor = (section == null) ? "-" : section.Title;
                } else
                {
                    textView.WantedSectionByAuthor = "-";
                }

                return textView;
            }
        } 

        // GET: Text
        public ActionResult Index()
        {
            List<Text> list; 
            if(Session["UserID"] == null || (String)Session["Role"] == "Registrirani korisnik")
            {
                list = db.Texts.Where(t => t.WebPublishable == true).ToList();
            } else if((String)Session["Role"] == "Autor") {
                int UserId = Int32.Parse((String)Session["UserID"]);
                list = db.Texts.Where(t => t.UserId == UserId).ToList();
            } else
            {
                list = db.Texts.ToList();
            }
            
            List<TextView> listView = new List<TextView>();

            foreach(Text t in list.ToList())
            {
                listView.Add(getTextView(t));
            }
            return View(listView);
        }

        public ActionResult ByAuthor(int id)
        {
            List<Text> list;
            if (Session["UserID"] == null || (String)Session["Role"] == "Registrirani korisnik")
            {
                list = db.Texts.Where(t => t.WebPublishable == true && t.UserId == id).ToList();
            }
            else
            {
                list = db.Texts.Where(t => t.UserId == id).ToList();
            }

            if (list.Count == 0)
            {
                TempData["Message"] = "Nema tekstova ovog autora.";
                return RedirectToAction("Index", "User");
            }
            List<TextView> listView = new List<TextView>();

            foreach (Text t in list)
            {
                listView.Add(getTextView(t));
            }
            return View(listView);
        }

        public ActionResult ForLectoring()
        {
            if((String)Session["Role"] != "Lektor")
            {
                TempData["Message"] = "Samo lektor može pristupiti ovoj stranici.";
                return RedirectToAction("Index", "Text");
            }

            List<Text> list = db.Texts.Where(t => t.TextStatus == (int)TextStatus.ACCEPTED).ToList();
            if (list.Count == 0)
            {
                TempData["Message"] = "Nema tekstova za lektoriranje.";
                return RedirectToAction("Index", "User");
            }
            List<TextView> listView = new List<TextView>();

            foreach (Text t in list)
            {
                listView.Add(getTextView(t));
            }
            return View(listView);
        }

        public ActionResult ForGraphicEditing()
        {
            List<Text> list = db.Texts.Where(t => t.TextStatus == (int)TextStatus.LECTORED).ToList();
            if (list.Count == 0)
            {
                TempData["Message"] = "Nema tekstova za grafičko uređivanje.";
                return RedirectToAction("Index", "User");
            }
            List<TextView> listView = new List<TextView>();

            foreach (Text t in list)
            {
                listView.Add(getTextView(t));
            }
            return View(listView);
        }

        // GET: Text/Details/5
        public ActionResult Details(int id)
        {
            if (db.Texts.Find(id).WebPublishable) {
                return View(getTextView(db.Texts.Single(u => u.TextId == id)));
            }
            if(Session["UserID"] != null && (String)Session["Role"] != "Registrirani korisnik")
            {
                return View(getTextView(db.Texts.Single(u => u.TextId == id)));
            }
            TempData["Message"] = "Ne možete vidjeti ovaj tekst.";
            return RedirectToAction("Index", "Text");
        }

     
        // GET: Text/Create
        public ActionResult Create()
        {
            if(Session["UserID"] == null)
            {
                TempData["Message"] = "Morate biti logirani da napišete tekst.";
                return RedirectToAction("Index");
            }
            ViewBag.DropDownList = new SelectList(db.Sections, "SectionId", "Title");
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
                TempData["Message"] = "Tekst je uspješno napisan.";

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            if(Session["UserID"] == null)
            {
                TempData["Message"] = "Ne možete mijenjati ovaj tekst.";
                return RedirectToAction("Details/" + id, "Text");
            }

            if((String)Session["Role"] == "Lektor" &&
                db.Texts.Find(id).TextStatus != (int)TextStatus.ACCEPTED)
            {
                return View(db.Texts.Single(t => t.TextId == id));
            }

            if(Int32.Parse((String)Session["UserID"]) == db.Texts.Find(id).UserId)
            {
                if(db.Texts.Find(id).TextStatus != (int)TextStatus.RETURNED)
                {
                    TempData["Message"] = "Ne možete mijenjati ovaj tekst.";
                    return RedirectToAction("Details/" + id, "Text");
                }

                return View(db.Texts.Single(t => t.TextId == id));
            } else
            {
                TempData["Message"] = "Samo autor može mijenjati ovaj tekst.";
                return RedirectToAction("Details/" + id, "Text");
            }
            
        }

        [HttpPost]
        public ActionResult Edit(int id, Text text)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var query = from ord in db.Texts
                            where ord.TextId == id
                            select ord;

                foreach (Text t in query)
                {
                    t.Content = text.Content;

                    if ((string)Session["Role"] == "Lektor")
                    {
                        t.TextStatus = (int)TextStatus.LECTORED;
                        TempData["Message"] = "Tekst je lektoriran.";
                    }
                    else
                    {
                        t.TextStatus = (int)TextStatus.SENT;
                        TempData["Message"] = "Tekst je ispravljen i ponovno poslan.";
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // GET: Text/EditEditor/5
        public ActionResult EditEditor(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                if (db.Ratings.Count(r => r.TextId == id) < 1)
                    //PROMIJENITI NA 5
                {
                    TempData["Message"] = "Svi članovi uredničkog vijeća moraju ocijeniti tekst.";
                    return RedirectToAction("Details/" + id, "Text");
                }
                var text = db.Texts.Single(d => d.TextId == id);

                ViewBag.DropDownListSections = new SelectList(db.Sections, "SectionId", "Title");
                return View(text);
            }
        }

        // POST: Text/EditEditor/5
        [HttpPost]
        public ActionResult EditEditor(int id, Text text, string submit)
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
                    t.Suggestions = text.Suggestions;
                    t.FinalSectionId = text.FinalSectionId;
                    t.FinalSection = text.FinalSection;

                    if (submit == "Vrati na doradu")
                    {
                        NotificationController.createNotification(t, "Vas tekst mora biti doraden po uputama.");
                        t.TextStatus = (int)TextStatus.RETURNED;
                    }
                    else if (submit == "Prihvati")
                    {
                        NotificationController.createNotification(t, "Vas tekst je prihvacen. Ostatak informacija nalazi se u detaljima teksta.");
                        NotificationController.createNotification(db.Roles.Single(r => r.RoleName == "Lektor").RoleId, t, "Tekst \"" + t.Title + "\" ceka vase lektoriranje.");
                        t.TextStatus = (int)TextStatus.ACCEPTED;
                    } else if (submit == "Odbij")
                    {
                        NotificationController.createNotification(t, "Vas tekst je odbijen. Detaljnije objasnjenje u obavijesti.");
                        t.TextStatus = (int)TextStatus.DECLINED;
                    }
                }

                db.SaveChanges();
                ViewBag.DropDownListSections = new SelectList(db.Sections, "SectionId", "Title");
                TempData["Message"] = "Obavijest o odluci je poslana autoru teksta.";
                return RedirectToAction("Index");
            }
        }

        // GET: Text/EditMember/5
        public ActionResult EditMember(int id)
        {
            return RedirectToAction("Create/" + id, "Ratings");
        }

        // GET: Text/Delete/5
        public ActionResult Delete(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                db.Texts.Remove(db.Texts.Find(id));
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //GET: Text/Download/<id>
        public ActionResult Download(int id)
        {

            if (db.Texts.Find(id).WebPublishable)
            {
                return createRTF(db.Texts.Single(u => u.TextId == id));
            }
            if (Session["UserID"] != null && (String)Session["Role"] != "Registrirani korisnik")
            {
                return createRTF((db.Texts.Single(u => u.TextId == id)));
            }
            TempData["Message"] = "Can not download this text.";
            return RedirectToAction("Index", "Text");
        }

        private FileContentResult createRTF(Text text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(text.Title).Append("\n\n");
            sb.Append(text.Content).Append("\n\n");
            sb.Append(text.Author.FirstName + " " + text.Author.LastName).Append("\n");
            sb.Append(text.Time.ToString()).Append("\n").Append("\n");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/plain", text.Title + ".rtf");
        }
    }
}
