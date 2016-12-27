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

                rView.Rate = rating.Rate;
                rView.SectionTitle = rating.SectionTitle;
                rView.TextId = rating.TextId;
                rView.UserId = rating.UserId;
                rView.Title = db.Texts.Find(rView.TextId).Title;
                rView.Username = db.Users.Find(rView.UserId).UserName;
                rView.Author = db.Users.Single(u => u.UserId == rView.UserId).UserName;
                rView.Subtitle = db.Texts.Find(rView.TextId).Subtitle;
                rView.Content = db.Texts.Find(rView.TextId).Content;
                rView.Time = db.Texts.Find(rView.TextId).Time;

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
            var ratings = db.Ratings.Include(r => r.Text).Include(r => r.User);
            return View(ratings.ToList());
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Role"] == null)
            {
                TempData["Message"] = "You must be logged in to rate.";
                return RedirectToAction("Details/" + id, "Text");
            }
            if((String)Session["Role"] != "Editor" && (String)Session["Role"] != "Editoral council member")
            {
                TempData["Message"] = "You must have priviledges to rate.";
                return RedirectToAction("Details/" + id, "Text");
            }

            var ratings = db.Ratings.Where(r => r.TextId == id);
            List<RatingView> ratingViews = new List<RatingView>();

            foreach(Rating rating in ratings)
            {
                ratingViews.Add(createRatingView(rating));
            }

            return View(ratingViews);
        }

        // GET: Ratings/Create/5
        public ActionResult Create(int id)
        {
            if(Session["UserID"] == null)
            {
                TempData["Message"] = "Must be logged in to rate.";
                return RedirectToAction("Details/" + id, "Text");
            }

            int userId = Int32.Parse((String)Session["UserID"]);
            ViewBag.UserName = db.Users.Single(u => u.UserId == userId).UserName;
            ViewBag.Title = db.Texts.Single(t => t.TextId == id).Title;

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

        // GET: Ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.TextId = new SelectList(db.Texts, "TextId", "Title", rating.TextId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", rating.UserId);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,TextId,WebPublishable,Rate,SectionTitle")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TextId = new SelectList(db.Texts, "TextId", "Title", rating.TextId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", rating.UserId);
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
