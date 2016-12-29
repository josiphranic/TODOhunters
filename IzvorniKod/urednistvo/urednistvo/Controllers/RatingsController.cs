﻿using System;
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
    public class RatingsController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        public RatingView createRatingView(Rating rating)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                RatingView rView = new RatingView();

                Rate rate = db.Rates.Single(r => r.Value == rating.Rate);
                if (rate == null) rView.Rate = "-";
                else rView.Rate = rate.Name;

                rView.SectionId = rating.SectionId;
                rView.SectionTitle = db.Sections.Find(rView.SectionId).Title;
                rView.TextId = rating.TextId;
                rView.UserId = rating.UserId;
                rView.Title = db.Texts.Find(rView.TextId).Title;
                rView.Username = db.Users.Find(rView.UserId).UserName;
                rView.Author = db.Users.Single(u => u.UserId == rView.UserId).UserName;
                rView.Subtitle = db.Texts.Find(rView.TextId).Subtitle;
                rView.Content = db.Texts.Find(rView.TextId).Content;
                rView.Time = db.Texts.Find(rView.TextId).Time;
                rView.WebPublishable = rating.WebPublishable;

                return rView;
            }
        }

        public RatingView createInitialRatingView(int TextId)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                RatingView rView = new RatingView();

                rView.Title = db.Texts.Find(TextId).Title;
                rView.Author = db.Texts.Single(u => u.TextId == TextId).Author.UserName;
                rView.Subtitle = db.Texts.Find(TextId).Subtitle;
                rView.Content = db.Texts.Find(TextId).Content;
                rView.Time = db.Texts.Find(TextId).Time;

                return rView;
            }
        }

        // GET: Ratings
        public ActionResult Index()
        {
            var ratings = db.Ratings.ToList();
            List<RatingView> rViews = new List<RatingView>();

            foreach (Rating rating in ratings)
            {
                rViews.Add(createRatingView(rating));
            }
            return View(rViews);
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int id)
        {
            /*if (Session["Role"] == null)
            {
                TempData["Message"] = "You must be logged in to rate.";
                return RedirectToAction("Details/" + id, "Text");
            }
            if((String)Session["Role"] != "Editor" && (String)Session["Role"] != "Editoral council member")
            {
                TempData["Message"] = "You must have priviledges to rate.";
                return RedirectToAction("Details/" + id, "Text");
            }*/

            // DODATI OGRANICENJA PRISTUPA OCJENAMA

            var ratings = db.Ratings.Where(r => r.TextId == id).ToList();

            if (ratings.Count == 0)
            {
                TempData["Message"] = "No ratings for this text.";
                return RedirectToAction("Index", "Text");
            }

            List<RatingView> rViews = new List<RatingView>();

            foreach (Rating rating in ratings)
            {
                rViews.Add(createRatingView(rating));
            }
            return View(rViews);
        }

        // GET: Ratings/Create/5
        public ActionResult Create(int id)
        {
            if (Session["UserID"] == null)
            {
                TempData["Message"] = "Must be logged in to rate.";
                return RedirectToAction("Details/" + id, "Text");
            }

            int userId = Int32.Parse((String)Session["UserID"]);
            if (db.Ratings.Count(r => r.UserId == userId && r.TextId == id) != 0)
            {
                TempData["Message"] = "You have already rated.";
                return RedirectToAction("Index", "Text");
            }

            ViewBag.DropDownListRates = new SelectList(db.Rates, "Value", "Name");
            ViewBag.DropDownListSections = new SelectList(db.Sections, "SectionId", "Title");
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(int id, Rating rating)
        {
            if (ModelState.IsValid)
            {
                rating.TextId = id;
                rating.UserId = Int32.Parse((String)Session["UserID"]);

                db.Ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
    } 
}
