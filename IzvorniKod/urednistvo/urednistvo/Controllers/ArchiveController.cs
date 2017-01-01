using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView.Textual;

namespace urednistvo.Controllers
{
    public class ArchiveController : Controller
    {
        // GET: Archive
        public ActionResult Index()
        {
            List<Text> list;
            DateTime date = DateTime.Today;
            DateTime dateArchive = date.AddDays(-14);
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                if (Session["UserID"] == null || (String)Session["Role"] == "Registrirani korisnik")
                {
                    list = db.Texts.Where(t => t.WebPublishable == true && t.Time.CompareTo(dateArchive) <= 0).ToList();
                }
                else if ((String)Session["Role"] == "Autor")
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
    }
}