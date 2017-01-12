using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private static int EDITORIAL_COUNCIL_MEMBERS = 5;

        public static TextView getTextView(Text text, bool copyContent)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                TextView textView = new TextView();
                var text = db.Texts.FirstOrDefault(t => t.TextId == textInput.TextId);

                textView.TextId = text.TextId;
                textView.Title = text.Title;
                textView.Subtitle = text.Subtitle;
                textView.Username = db.Users.Single(u => u.UserId == text.UserId).UserName;
                textView.UserId = text.UserId;
                textView.Time = text.Time;
                if(copyContent) textView.Content = text.Content;
                textView.TextStatus = TextStatusNameGetter.getName(text.TextStatus);
                textView.WebPublishable = text.WebPublishable.ToString();
                textView.EditionPublishable = text.EditionPublishable.ToString();
                textView.FinalSectionId = text.FinalSectionId;
                textView.WantedSectionByAuthorId = text.WantedSectionByAuthorId;
                textView.RatingsCount = text.Ratings == null ? 0 : text.Ratings.Count();

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

                if (text.Suggestions == null)
                {
                    textView.Suggestions = "-";
                } else
                {
                    textView.Suggestions = text.Suggestions;
                }

                return textView;
            }
        }

        // GET: Text
        public ActionResult Index()
        {
            DateTime dateArchive = DateTime.Today.AddDays(-7);
            List<Text> list;
            if (Session["UserID"] == null || (String)Session["Role"] == RoleNames.REGISTERED_USER)
            {
                list = db.Texts.Where(t => t.WebPublishable == true).ToList();
            } else if ((String)Session["Role"] == RoleNames.AUTHOR) {
                int UserId = Int32.Parse((String)Session["UserID"]);
                list = db.Texts.Where(t => t.UserId == UserId || t.WebPublishable).ToList();
            } else
            {
                list = db.Texts.ToList();
            }

            List<TextView> listView = new List<TextView>();

            foreach (Text t in list.ToList())
            {
                listView.Add(getTextView(t, false));
            }
            return View(listView);
        }

        public ActionResult ByAuthor(int id)
        {
            List<Text> list;
            if (Session["UserID"] == null || (String)Session["Role"] == RoleNames.REGISTERED_USER)
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
                listView.Add(getTextView(t, false));
            }
            return View(listView);
        }

        public ActionResult ForLectoring()
        {
            if ((String)Session["Role"] != RoleNames.LECTOR)
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
                listView.Add(getTextView(t, false));
            }
            return View(listView);
        }

        public ActionResult ForAddingImages()
        {
            if ((String)Session["Role"] != RoleNames.EDITOR)
            {
                TempData["Message"] = "Samo glavni urednik može pristupiti ovoj stranici.";
                return RedirectToAction("Index", "Text");
            }

            List<Text> list = db.Texts.Where(t => t.TextStatus == (int)TextStatus.LECTORED && t.EditionPublishable == true).ToList();
            if (list.Count == 0)
            {
                TempData["Message"] = "Nema tekstova za dodavanje slika.";
                return RedirectToAction("Index", "User");
            }
            List<TextView> listView = new List<TextView>();

            foreach (Text t in list)
            {
                listView.Add(getTextView(t, false));
            }
            return View(listView);
        }

        public ActionResult ForRate()
        {
            if ((String)Session["Role"] != RoleNames.EDITOR && (String)Session["Role"] != RoleNames.EDITORIAL_COUNCIL_MEMBER)
            {
                TempData["Message"] = "Samo članovi uredničkog vijeća mogu pristupiti ovoj stranici.";
                return RedirectToAction("Index", "Text");
            }

            List<Text> list = db.Texts.Where(t => t.TextStatus == (int)TextStatus.SENT).ToList();
            
            if (list.Count == 0)
            {
                TempData["Message"] = "Nema tekstova za lektoriranje.";
                return RedirectToAction("Index", "User");
            }
            List<TextView> listView = new List<TextView>();

            foreach (Text t in list)
            {
                int ratesCount = db.Ratings.Where(r => r.TextId == t.TextId).Count();
                if (ratesCount < EDITORIAL_COUNCIL_MEMBERS)
                {
                    listView.Add(getTextView(t, false));
                }
            }
            return View(listView);
        }

        public ActionResult ForGraphicEditing()
        {
            List<Text> list = db.Texts.Where(t => t.TextStatus == (int)TextStatus.ADDED_PICS).ToList();
            if (list.Count == 0)
            {
                TempData["Message"] = "Nema tekstova za grafičko uređivanje.";
                return RedirectToAction("Index", "User");
            }
            List<TextView> listView = new List<TextView>();

            foreach (Text t in list)
            {
                listView.Add(getTextView(t, false));
            }
            return View(listView);
        }

        public ActionResult ForCorrection()
        {
            List<Text> list = db.Texts.Where(t => t.TextStatus == (int)TextStatus.GRAPHIC).ToList();
            if (list.Count == 0)
            {
                TempData["Message"] = "Nema tekstova za korekturu.";
                return RedirectToAction("Index", "User");
            }
            List<TextView> listView = new List<TextView>();

            foreach (Text t in list)
            {
                listView.Add(getTextView(t, false));
            }
            return View(listView);
        }

        // GET: Text/Details/5
        public ActionResult Details(int id)
        {
            if (db.Texts.Find(id).WebPublishable) {
                return View(getTextView(db.Texts.Single(u => u.TextId == id), true));
            }
            if(Session["UserID"] != null && (String)Session["Role"] != RoleNames.REGISTERED_USER)
            {
                return View(getTextView(db.Texts.Single(u => u.TextId == id), true));
            }
            TempData["Message"] = "Ne možete vidjeti ovaj tekst.";
            return RedirectToAction("Index", "Text");
        }

     
        // GET: Text/Create
        public ActionResult Create()
        {
            if(Session["UserID"] == null || (String)Session["Role"] != RoleNames.AUTHOR)
            {
                TempData["Message"] = "Morate biti prijavljeni kao autor da napišete tekst.";
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

            if((String)Session["Role"] == RoleNames.LECTOR &&
                db.Texts.Find(id).TextStatus == (int)TextStatus.ACCEPTED)
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

                    if ((string)Session["Role"] == RoleNames.LECTOR)
                    {
                        t.TextStatus = (int)TextStatus.LECTORED;
                        TempData["Message"] = "Tekst je lektoriran.";
                        NotificationController.createNotificationForEditor(t, "Tekst \" " + t.Title + "\" je lektoriran. Možete priložiti slike uz tekst.");
                    }
                    else
                    {
                        t.TextStatus = (int)TextStatus.SENT;
                        TempData["Message"] = "Tekst je ispravljen i ponovno poslan.";
                        NotificationController.createNotificationForEditor(t, "Tekst \" " + t.Title + "\" je ponovo poslan");
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
                if (db.Ratings.Count(r => r.TextId == id) < EDITORIAL_COUNCIL_MEMBERS)
                {
                    TempData["Message"] = "Svi članovi uredničkog vijeća moraju ocijeniti tekst.";
                    return RedirectToAction("Details/" + id, "Text");
                }
                var text = db.Texts.Single(d => d.TextId == id);

                Section s = db.Sections.Find(text.WantedSectionByAuthorId);
                return View(new Tuple<Text, string>(text, s == null ? "-" : s.Title));
            }
        }

        // POST: Text/EditEditor/5
        [HttpPost]
        public ActionResult EditEditor(int id, Text text, string submit)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Text t = db.Texts.Find(id);

                t.Suggestions = text.Suggestions;

                if (submit == "Vrati na doradu")
                {
                    NotificationController.createNotificationForAuthor(t, "Vaš tekst mora biti dorađen po uputama. Više informacija nalazi se u detaljnijem pregledu teksta.");
                    t.TextStatus = (int)TextStatus.RETURNED;
                }
                else if (submit == "Prihvati")
                {
                    t.EditionPublishable = text.EditionPublishable;
                    t.WebPublishable = text.WebPublishable;
                    t.FinalSectionId = text.FinalSectionId;
                    t.FinalSection = text.FinalSection;

                    NotificationController.createNotificationForAuthor(t, "Vas tekst je prihvaćen. Više informacija nalazi se u detaljižnijem pregledu teksta.");
                    NotificationController.createNotificationForLector(t);
                    t.TextStatus = (int)TextStatus.ACCEPTED;
                } else if (submit == "Odbij")
                {
                    t.EditionPublishable = false;
                    t.WebPublishable = false;
                    t.FinalSectionId = -1;
                    t.FinalSection = null;
                    NotificationController.createNotificationForAuthor(t, "Vas tekst je odbijen. Više informacija nalazi se u detaljižnijem pregledu teksta.");
                    t.TextStatus = (int)TextStatus.DECLINED;
                }

                db.SaveChanges();
                ViewBag.DropDownListSections = new SelectList(db.Sections, "SectionId", "Title");
                TempData["Message"] = "Obavijest je poslana autoru teksta.";

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
            if((string)Session["Role"] != RoleNames.EDITOR)
            {
                TempData["Message"] = "Nemate prava za brisanje teksta.";
                return RedirectToAction("Index", "Text");
            }

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
            if (Session["UserID"] != null && (String)Session["Role"] != RoleNames.REGISTERED_USER)
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

        [HttpPost]
        public ActionResult UploadRTF(int? id, HttpPostedFileBase uploadFile)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    string name = System.IO.Path.GetFileName(uploadFile.FileName);
                    string type = name.Substring(name.LastIndexOf(".") + 1);

                    if (type != "rtf")
                    {
                        TempData["Message"] = "Krivi oblik datoteke.";
                        return RedirectToAction("Index");
                    }

                    List<Pdf> pdfs = db.Pdfs.Where(p => p.TextId == (int)id).ToList();
                    foreach(Pdf p in pdfs)
                    {
                        System.IO.File.Delete(System.IO.Path.Combine(
                                           Server.MapPath("~/RTFs"), p.PdfName));
                        db.Pdfs.Remove(p);
                        db.SaveChanges();
                    }

                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/RTFs"), name);

                    uploadFile.SaveAs(path);


                    Pdf pdf = new Pdf();
                    pdf.PdfName = name;
                    pdf.TextId = (int)id;
                    pdf.Text = db.Texts.Find(id);
                    db.Pdfs.Add(pdf);
                    db.SaveChanges();              
                    
                    if ((string)Session["Role"] == RoleNames.GRAPHIC_EDITOR)
                    {
                        NotificationController.createNotificationForCorrector(db.Texts.Find(id));

                        Text t = db.Texts.Find(id);
                        t.TextStatus = (int)TextStatus.GRAPHIC;
                        db.SaveChanges();

                        return RedirectToAction("ForGraphicEditing/" + id);
                    }
                    else if ((string)Session["Role"] == RoleNames.CORRECTOR)
                    {
                        Text t = db.Texts.Find(id);
                        t.TextStatus = (int)TextStatus.CORRECTED;
                        db.SaveChanges();

                        return RedirectToAction("ForCorrection/" + id);
                    }
                }

                return RedirectToAction("Index");
            }
        }

        public ActionResult DownloadRTF(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Pdf> pdfs = db.Pdfs.Where(p => p.TextId == (int)id).ToList();
            if (pdfs.Count == 0)
            {
                TempData["Message"] = "Ovaj tekst nema RTF dokument.";
                return RedirectToAction("Index", "Text");
            }

            Pdf pdf = pdfs.First();
            string path = Server.MapPath("~/RTFs/" + pdf.PdfName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, pdf.PdfName);
        }

        public ActionResult AnnouncementTexts()
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                DateTime lastPublised = DateTime.Now.AddYears(-100);

                List<Edition> editions = db.Editions.ToList();
                foreach (Edition edition in editions)
                {
                    if (DateTime.Compare(edition.TimeOfRelease, lastPublised) > 0)
                    {
                        lastPublised = edition.TimeOfRelease;
                    }
                }

                List<Text> texts = db.Texts.Where(t => DateTime.Compare(t.Time, lastPublised) > 0).ToList();
                List<TextView> textViews = new List<TextView>();

                foreach (Text text in texts)
                {
                    textViews.Add(getTextView(text, false));
                }

                return View(textViews);
            }
        }

        [ChildActionOnly]
        public ActionResult PartialAnnouncements()
        {
            // OVO TREBA POSLOZITI PO NACELIMA OOPA!!!!!! KOPIRAN KOD OD GORE
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                DateTime lastPublised = DateTime.Now.AddYears(-100);

                List<Edition> editions = db.Editions.ToList();
                foreach (Edition edition in editions)
                {
                    if (DateTime.Compare(edition.TimeOfRelease, lastPublised) > 0)
                    {
                        lastPublised = edition.TimeOfRelease;
                    }
                }

                List<Text> texts = db.Texts.Where(t => DateTime.Compare(t.Time, lastPublised) > 0).ToList();
                List<TextView> textViews = new List<TextView>();

                foreach (Text text in texts)
                {
                    textViews.Add(getTextView(text, false));
                }

                return PartialView("_Announcements", textViews);
            }
        }

        public ActionResult EndAddingPic(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Text text = db.Texts.Find(id);
            if (text == null)
            {
                return HttpNotFound();
            }

            text.TextStatus = (int)TextStatus.ADDED_PICS;
            db.SaveChanges();
            return RedirectToAction("Index", "Image");
        }
    }
}
