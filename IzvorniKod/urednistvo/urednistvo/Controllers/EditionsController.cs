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
    public class EditionsController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        private EditionView createEditionView(Edition edition)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                EditionView eView = new EditionView();
                eView.EditionId = edition.EditionId;
                eView.Title = edition.Title;
                eView.TimeOfRelease = edition.TimeOfRelease;
                eView.StartTime = eView.TimeOfRelease.AddDays(-7);
                eView.NumberOfTexts = db.Texts.Where(t => t.Time < eView.TimeOfRelease &&
                                                            t.Time > eView.StartTime).Count();
                return eView;
            }
        }

        // GET: Editions
        public ActionResult Index()
        {
            List<EditionView> eViews = new List<EditionView>();
            foreach (Edition e in db.Editions.ToList())
            {
                eViews.Add(createEditionView(e));
            }
            return View(eViews);
        }

        // GET: Editions/Details/5
        public ActionResult Details(int? id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                Edition edition = db.Editions.Single(e => e.EditionId == id);
                DateTime end = edition.TimeOfRelease;
                DateTime start = edition.TimeOfRelease.AddDays(-7);

                List<Section> sections = db.Sections.ToList();
                List<SectionView> sViews = new List<SectionView>();

                foreach (Section s in sections)
                {
                    sViews.Add(SectionsController.createSectionView(s, start, end));
                }

                return View(sViews);
            }
        }

        // GET: Editions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Editions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Edition edition)
        {
            if (ModelState.IsValid)
            {
                if(edition.Title == null)
                {
                    TempData["Message"] = "Tiskovina mora imati naslov.";
                    return RedirectToAction("Create", "Editions");
                }
                edition.TimeOfRelease = DateTime.Now;
                db.Editions.Add(edition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(edition);
        }
    }
}
