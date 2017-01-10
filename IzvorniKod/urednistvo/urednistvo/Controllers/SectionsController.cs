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
    public class SectionsController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        private static SectionView createSectionView(Section section)
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

        public static SectionView createSectionView(Section section, DateTime start, DateTime end)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                SectionView sView = new SectionView();
                sView.SectionId = section.SectionId;
                sView.Title = section.Title;
                sView.NumberOfTexts = db.Texts.Count(t => ((t.WebPublishable == true && t.TextStatus == (int)TextStatus.LECTORED) ||
                                                    (t.EditionPublishable == true && t.TextStatus == (int)TextStatus.CORRECTED)) &&
                                                        t.Time < end &&
                                                        t.Time > start && t.FinalSectionId == section.SectionId);

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
            if(list.Count == 0)
            {
                TempData["Message"] = "Nema tekstova u ovom podlistku.";
                return RedirectToAction("Index");
            }

            List<TextView> textViews = new List<TextView>();

            foreach (Text t in list)
            {
                textViews.Add(TextController.getTextView(t));
            }

            return View(textViews);
        }

        // GET: Sections/Details/5
        public ActionResult DetailsByEdition(int SectionId, int EditionId)
        {
            Edition edition = db.Editions.Find(EditionId);

            if(edition == null)
            {
                TempData["Message"] = "Greska kod trazenja teksta.";
                RedirectToAction("Details/" + SectionId);
            }

            DateTime end = edition.TimeOfRelease;
            DateTime start = edition.TimeOfRelease.AddDays(-7);

            List<Text> list = db.Texts.Where(t => ((t.WebPublishable == true && t.TextStatus == (int)TextStatus.LECTORED) ||
                                                    (t.EditionPublishable == true && t.TextStatus == (int)TextStatus.CORRECTED)) &&
                                                        t.Time < end &&
                                                        t.Time > start && t.FinalSectionId == SectionId).ToList();
            if (list.Count == 0)
            {
                TempData["Message"] = "Nema tekstova u ovom podlistku za odabrano izdanje.";
                return RedirectToAction("Index");
            }

            List<TextView> textViews = new List<TextView>();

            foreach (Text t in list)
            {
                textViews.Add(TextController.getTextView(t));
            }

            return View(textViews);
        }
    }
}
