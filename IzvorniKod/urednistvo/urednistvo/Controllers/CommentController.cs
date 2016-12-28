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
    public class CommentController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        public CommentView createCommentView(Comment comment)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {

                CommentView cView = new CommentView();

                cView.Content = comment.Content;
                cView.TextId = comment.TextId;
                cView.TextTitle = db.Texts.Single(t => t.TextId == comment.TextId).Title;
                cView.Time = comment.Time;
                cView.UserId = comment.UserId;
                cView.Username = db.Users.Single(u => u.UserId == comment.UserId).UserName;

                return cView;
            }
        }


        // GET: Comments
        public ActionResult Index()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Comments
        public ActionResult ByText(int id)
        {
            var query = from ord in db.Comments
                        where ord.TextId == id
                        select ord;

            if(query.Count() == 0)
            {
                TempData["Message"] = "No comments for this text.";
                return RedirectToAction("Index", "Text");
            }

            List<CommentView> commentViews = new List<CommentView>();
            foreach(Comment comment in query)
            {
                commentViews.Add(createCommentView(comment));
            }
            return View(commentViews);
        }

        public ActionResult ByUser(int id)
        {
            if (db.Ratings.Count(r => r.UserId == id) == 0)
            {
                TempData["Message"] = "No comments for this user.";
                return RedirectToAction("Index", "Text");
            }

            var query = from ord in db.Comments
                        where ord.UserId == id
                        select ord;

            List<CommentView> commentViews = new List<CommentView>();
            foreach(Comment comment in query)
            {
                commentViews.Add(createCommentView(comment));
            }
            return View(commentViews);
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create/5
        public ActionResult Create(int id)
        {
            if (Session["UserID"] == null)
            {
                TempData["Message"] = "Registrirajte se ili logirajte da bi komentirali.";
                return RedirectToAction("Details/" + id, "Text");
            }
            return View();
        }

        // POST: Comments/Create/5
        [HttpPost]
        public ActionResult Create(int id, Comment comment)
        {
            if (ModelState.IsValid)
            {
                using (UrednistvoDatabase db = new UrednistvoDatabase())
                {
                    
                    comment.Time = DateTime.Now;
                    comment.UserId = Int32.Parse((String)Session["UserID"]);
                    comment.TextId = id;

                    //return Content(comment.Content + " " + comment.Time + " " + comment.UserId + " " + comment.TextId);

                    db.Comments.Add(comment);
                    db.SaveChanges();
                }
                ModelState.Clear();
                TempData["Message"] = "Komentar je uspjesno stvoren.";

                return RedirectToAction("ByText/" + comment.TextId);
            }
            return View();
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TextId = new SelectList(db.Texts, "TextId", "Title", comment.TextId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,TextId,Content,Time")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TextId = new SelectList(db.Texts, "TextId", "Title", comment.TextId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
