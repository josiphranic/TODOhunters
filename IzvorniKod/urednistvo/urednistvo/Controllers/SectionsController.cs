using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView.Textual;

namespace urednistvo.Controllers
{
    public class SectionsController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        private SectionView createSectionView(Section section)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                SectionView sView = new SectionView();
                sView.SectionId = section.SectionId;
                sView.Title = section.Title;
                sView.NumberOfTexts = db.Texts.Count(t => t.FinalSectionId == sView.SectionId);

                return sView;
            }
        }

        // GET: Sections
        public ActionResult Index()
        {
            List<SectionView> list = new List<SectionView>();
            foreach (Section s in db.Sections.ToList())
            {
                list.Add(createSectionView(s));
            }
            return View(list);
        }

        // GET: Sections/Details/5
        public ActionResult Details(int id)
        {
            List<Text> list = db.Texts.Where(s => s.FinalSectionId == id).ToList();
            List<TextView> textViews = new List<TextView>();

            foreach (Text t in list)
            {
                textViews.Add(TextController.getTextView(t));
            }

            return View(textViews);
        }

        // GET: Sections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectionId,Title")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(section);
        }
    }
}
