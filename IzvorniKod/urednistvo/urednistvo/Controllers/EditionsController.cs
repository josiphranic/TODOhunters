using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;
using urednistvo.ModelsView.Textual;

namespace urednistvo.Controllers
{
    public class EditionsController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        public static EditionView createEditionView(Edition edition)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                EditionView eView = new EditionView();
                eView.EditionId = edition.EditionId;
                eView.Title = edition.Title;
                eView.TimeOfRelease = edition.TimeOfRelease;
                eView.StartTime = eView.TimeOfRelease.AddDays(-7);
                eView.NumberOfTexts = db.Texts.Where(t => ((t.WebPublishable == true && t.TextStatus == (int)TextStatus.LECTORED) ||
                                                    (t.EditionPublishable == true && t.TextStatus == (int)TextStatus.CORRECTED)) &&
                                                        t.Time < eView.TimeOfRelease &&
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

                return View(new Tuple<List<SectionView>, Edition>(sViews, edition));
            }
        }

        public ActionResult EditionTexts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Edition edition = db.Editions.Find((int)id);
            if (edition == null)
            {
                return HttpNotFound();
            }

            EditionView eView = createEditionView(edition);
            List<Text> texts = db.Texts.Where(t => ((t.WebPublishable == true && t.TextStatus == (int)TextStatus.LECTORED) ||
                                                    (t.EditionPublishable == true && t.TextStatus == (int)TextStatus.CORRECTED)) &&
                                                        t.Time < eView.TimeOfRelease &&
                                                        t.Time > eView.StartTime).ToList();

            if(texts.Count == 0)
            {
                TempData["Message"] = "Nema tekstova u ovom izdanju.";
                return RedirectToAction("Index");
            }

            List<TextView> textViews = new List<TextView>();
            foreach(Text t in texts)
            {
                textViews.Add(TextController.getTextView(t));
            }

            return View(textViews);
        }
    }
}
