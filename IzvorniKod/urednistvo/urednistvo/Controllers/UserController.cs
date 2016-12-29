using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;
using urednistvo.ModelsView.Utilities;

namespace urednistvo.Controllers
{
    public class UserController : Controller
    {
        private UrednistvoDatabase db = new UrednistvoDatabase();

        public UserView createUserView(User user)
        {
            UserView uView = new UserView();

            uView.UserId = user.UserId;
            uView.Email = user.Email;
            uView.FirstName = user.FirstName;
            uView.LastName = user.LastName;
            uView.Role = RoleNameGetter.getName(user.Role);
            uView.UserName = user.UserName;

            return uView;
        }

        // GET: User
        public ActionResult Index()
        {
            using(UrednistvoDatabase db = new UrednistvoDatabase())
            {
                List<UserView> userViews = new List<UserView>();

                foreach(User u in db.Users.ToList())
                {
                    userViews.Add(createUserView(u));
                }
                return View(userViews);
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
                Session["UserID"] = account.UserId.ToString();
                Session["Username"] = account.UserName.ToString();
                Session["Role"] = RoleNameGetter.getName(account.Role);

                TempData["Message"] = account.FirstName + " " + account.LastName + " succesfully registered.";
            }
            return RedirectToAction("Index", "Home");
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
                    Session["Role"] = RoleNameGetter.getName(user.Role);

                    TempData["Message"] = "Successful login";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username or password wrong");
                }
            }
            return View();
        }

        public ActionResult Logoff()
        {
            Session["UserID"] = null;
            Session["Username"] = null;
            Session["Role"] = null;

            TempData["Message"] = "Successfully logged off";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details(int id)
        {
            using (UrednistvoDatabase db = new UrednistvoDatabase())
            {
                var user = db.Users.Single(d => d.UserId == id);
                return View(createUserView(user));
            }
        }
    }
}