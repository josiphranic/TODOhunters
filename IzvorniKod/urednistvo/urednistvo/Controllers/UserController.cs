using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;

namespace urednistvo.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            using(UrednistvoDatabase db = new UrednistvoDatabase())
            {
                return View(db.Users.ToList());
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User account)
        {
            if (ModelState.IsValid)
            {
                using (UrednistvoDatabase db = new UrednistvoDatabase())
                {
                    db.Users.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + " succesfully registered.";
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User account)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var query = from u in db.Users
                            where u.UserName.Equals(account.UserName) &&
                                u.Password.Equals(account.Password)
                            select u;
                var user = query.FirstOrDefault();
                if(user != null)
                {
                    Session["UserID"] = user.UserId.ToString();
                    Session["Username"] = user.UserName.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Username or password wrong");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if(Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logoff()
        {
            Session["UserID"] = null;
            Session["Username"] = null;

            ViewBag.Message = "Successfully logged off";
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var user = db.Users.Single(d => d.UserId == id);
                return View(user);
            }
        }
    }
}