using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView.Utilities;

namespace urednistvo.Controllers
{
    public class ImageController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        // GET: Image
        public ActionResult Index()
        {
            if((String)Session["Role"] != RoleNames.EDITOR && (String)Session["Role"] != RoleNames.GRAPHIC_EDITOR)
            {
                TempData["Message"] = "Nemate ovlati pristupiti ovoj stranici.";
                return RedirectToAction("Index", "Text");
            }

            return View(db.Images.ToList());
        }

        // GET: Image/Details/5
        public ActionResult Details(int? id)
        {
            if ((String)Session["Role"] != RoleNames.EDITOR && (String)Session["Role"] != RoleNames.GRAPHIC_EDITOR)
            {
                TempData["Message"] = "Nemate ovlati pristupiti ovoj stranici.";
                return RedirectToAction("Index", "Text");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        public ActionResult ByText(int? id)
        {
            if ((String)Session["Role"] != RoleNames.EDITOR && (String)Session["Role"] != RoleNames.GRAPHIC_EDITOR)
            {
                TempData["Message"] = "Nemate ovlati pristupiti ovoj stranici.";
                return RedirectToAction("Index", "Text");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Text text = db.Texts.Find(id);
            if (text == null)
            {
                return HttpNotFound();
            }

            var images = db.Images.Where(i => i.TextId == id);
            if(images.Count() == 0)
            {
                TempData["Message"] = "Nema slika za ovaj tekst.";
                return RedirectToAction("Index");
            }

            return View(images.ToList());
        }

        // GET: Image/Create
        public ActionResult Create()
        {
            if ((String)Session["Role"] != RoleNames.EDITOR && (String)Session["Role"] != RoleNames.GRAPHIC_EDITOR)
            {
                TempData["Message"] = "Nemate ovlati pristupiti ovoj stranici.";
                return RedirectToAction("Index", "Text");
            }

            ViewBag.TextId = new SelectList(db.Texts, "TextId", "Title");
            return View();
        }

        // POST: Image/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Image image, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string type = pic.Substring(pic.LastIndexOf(".")+1);

                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Images"), image.ImageName + "." + type);

                file.SaveAs(path);

                image.Type = type;
                db.Images.Add(image);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public ActionResult Download(int? id)
        {
            if ((String)Session["Role"] != RoleNames.EDITOR && (String)Session["Role"] != RoleNames.GRAPHIC_EDITOR)
            {
                TempData["Message"] = "Nemate ovlati pristupiti ovoj stranici.";
                return RedirectToAction("Index", "Text");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            string path = Server.MapPath("~/Images/" + image.ImageName + "." + image.Type);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = image.ImageName + "." + image.Type;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
