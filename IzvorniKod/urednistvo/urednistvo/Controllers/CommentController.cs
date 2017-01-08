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

        public static CommentView createCommentView(Comment comment)
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
                TempData["Message"] = "Nema kometara za ovaj tekst.";
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
                TempData["Message"] = "Nema komentara ovog korisnika.";
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
    }
}
