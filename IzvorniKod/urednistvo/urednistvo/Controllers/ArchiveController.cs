using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;
using urednistvo.ModelsView.Textual;
using urednistvo.ModelsView.Utilities;

namespace urednistvo.Controllers
{
    public class ArchiveController : Controller
    {
        // GET: Archive
        public ActionResult Index()
        {
            List<Text> list;
     
            DateTime dateArchive = DateTime.Today.AddDays(-14);
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                if (Session["UserID"] == null || (String)Session["Role"] == RoleNames.REGISTERED_USER)
                {
                    list = db.Texts.Where(t => t.WebPublishable == true && t.Time.CompareTo(dateArchive) <= 0).ToList();
                }
                else if ((String)Session["Role"] == RoleNames.AUTHOR)
                {
                    int UserId = Int32.Parse((String)Session["UserID"]);
                    list = db.Texts.Where(t => t.UserId == UserId && t.Time.CompareTo(dateArchive) <= 0).ToList();
                }
                else
                {
                    list = db.Texts.Where(t => t.Time.CompareTo(dateArchive) <= 0 ).ToList();
                }
            }

            List<TextView> listView = new List<TextView>();
            foreach (Text t in list.ToList())
            {
                listView.Add(TextController.getTextView(t));
            }
            return View(listView);
        }

        //GET: Archive/Upload
        public ActionResult Upload()
        {
            if (!((String)Session["Role"] == RoleNames.EDITOR))
            {
                TempData["Message"] = "Samo glavni urednik može dodati tekst u arhivu";
                return RedirectToAction("Index", "Archive");
            }
            return View();
        }

        //POST: Archive/Upload
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if(file == null)
            {
                TempData["Message"] = "Tekst mora biti odabran.";
                return RedirectToAction("Upload", "Archive");
            }
            string fileExtension = Path.GetExtension(file.FileName);
            if(!fileExtension.Equals(".rtf"))
            {
                TempData["Message"] = "Tekst je neispravnog formata.";
                return RedirectToAction("Upload", "Archive");
            }

            string stringText = string.Empty;
            using(BinaryReader br = new BinaryReader(file.InputStream))
            {
                byte[] binData = br.ReadBytes(file.ContentLength);
                stringText = System.Text.Encoding.UTF8.GetString(binData);
            }

            Text text = parseText(stringText);
            if(text != null)
            {
               using(UrednistvoDatabase db = new UrednistvoDatabase ())
                {
                    db.Texts.Add(text);
                    db.SaveChanges();
                } 
                TempData["Message"] = "Tekst je uspješno doda u arhivu.";
            }
            return RedirectToAction("Upload" , "Archive");
        }

        private Text parseText(string stringText)
        {
            Text newText = new Text();
            StringBuilder sb = new StringBuilder();
            using (StringReader sr = new StringReader(stringText))
            {
                int mode = 1;
                string line = string.Empty;
                

                while ((line = sr.ReadLine()) != null)
                {
                    if (mode == 1)
                    {
                        newText.Title = line.Trim();
                        mode = 2;
    
                    }
                    else if (mode == 2)
                    {
                        DateTime date;
                        if (DateTime.TryParseExact(line.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            if(date.CompareTo(DateTime.Today) < 0)
                            {
                                newText.Time = date;
                                mode = 3;
                            } else
                            {
                                TempData["Message"] = "Datum je neispravan";
                                return null;
                            }
                        }
                        else
                        {
                            TempData["Message"] = "Datum je neispravno zadan.";
                            return null;
                        }
                    }
                    else if (mode == 3)
                    {
                        if(line.Trim() == String.Empty)
                        {
                            mode = 4;
                        } else
                        {
                            TempData["Message"] = "Tekst je neispravnog oblika.";
                            return null;
                        }
                    }
                    else if (mode == 4)
                    {
                        sb.Append(line).Append("\n");
                    } 
                }
            }
            newText.Content = sb.ToString();
            
            newText.TextStatus = (int)(int)TextStatus.ACCEPTED;
            newText.WebPublishable = true;
            newText.EditionPublishable = false;
            newText.FinalSectionId = -1;
            newText.UserId = Int32.Parse((String)(Session["UserID"]));

            return newText;
        }

        // GET: Archive/Text/5
        public ActionResult Text(int id)
        {
            using(UrednistvoDatabase db =  new UrednistvoDatabase())
            {
                if (db.Texts.Find(id).WebPublishable)
                {
                    return View(TextController.getTextView(db.Texts.Single(u => u.TextId == id)));
                }
                if (Session["UserID"] != null && (String)Session["Role"] != RoleNames.REGISTERED_USER)
                {
                    return View(TextController.getTextView(db.Texts.Single(u => u.TextId == id)));
                }
            }
            
            TempData["Message"] = "Ne možete vidjeti ovaj tekst.";
            return RedirectToAction("Index", "Archive");
        }

        //GET: Archive/Author/id
        public ActionResult Author(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var user = db.Users.Single(d => d.UserId == id);
                return View(UserController.createUserView(user));
            }
        }

        // GET: Archive/Comment/TextId
        public ActionResult Comment(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var query = from ord in db.Comments
                            where ord.TextId == id
                            select ord;

                if (query.Count() == 0)
                {
                    TempData["Message"] = "Nema kometara za ovaj tekst.";
                    return RedirectToAction("Index", "Archive");
                }

                List<CommentView> commentViews = new List<CommentView>();
                foreach (Comment comment in query)
                {
                    commentViews.Add(CommentController.createCommentView(comment));
                }
                return View(commentViews);
            }
               
        }
    }
}