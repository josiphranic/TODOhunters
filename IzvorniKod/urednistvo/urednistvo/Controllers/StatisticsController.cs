﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urednistvo.Models;
using urednistvo.ModelsView;
using urednistvo.ModelsView.Textual;

namespace urednistvo.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            if ((String)Session["Role"] == "Glavni urednik" || (String)Session["Role"] == "Član uredničkog vijeća")
            {
                TempData["Message"] = "Samo glavni urednik i članovi uredničko vijeća imaju pristup statistikama.";
            }
            return View();
        }

        //Get: Statistics/DataForRange
        public ActionResult DataForRange()
        {
            return View();
        }

        //Post: Statistics/DataForRange
        [HttpPost]
        public ActionResult DataForRange(DateTime from, DateTime to)
        {
            if(from.CompareTo(to) >= 0)
            {
                TempData["Message"] = "Odabrani su neispravni datumi";
                return RedirectToAction("Index", "Statistics");
            }
     
            StatisticsView stat = new StatisticsView();
            stat.to = to;
            stat.from = from;

            using(UrednistvoDatabase db = new UrednistvoDatabase())
            {
                List<EditionView> eViews = new List<EditionView>();
                foreach (Edition e in db.Editions.ToList())
                {
                    if(e.TimeOfRelease.CompareTo(from) >= 0 && e.TimeOfRelease.CompareTo(to) <= 0)
                    {
                        eViews.Add(EditionsController.createEditionView(e));
                    }
                    
                }
                stat.numEditions = eViews.Count();
                stat.editions = eViews;

                stat.numTexts = 0;
                SortedDictionary<UserView, Int32> uViewInt = new SortedDictionary<UserView, Int32>();
                foreach(User u in db.Users.ToList())
                {
                    if(db.Roles.Find(u.Role).RoleName.Equals("Autor") || db.Roles.Find(u.Role).RoleName.Equals("Glavni urednik"))
                    {
                        UserView user = UserController.createUserView(u);
                        uViewInt.Add(user, 0);
                        foreach(Text t in u.Texts)
                        {
                            if(t.Time.CompareTo(from) >= 0 && t.Time.CompareTo(to) <= 0)
                            {
                                uViewInt[user]++;
                                stat.numTexts++;
                            }
                        }
                    }
                }
                stat.authors = uViewInt;

                List<TextView> tView = new List<TextView>();
                foreach(Text t in db.Texts.ToList())
                {
                    if(t.Time.CompareTo(from) >= 0 && t.Time.CompareTo(to) <= 0)
                    {
                        tView.Add(TextController.getTextView(t));
                    }
                }
                stat.texts = tView;
                stat.numTexts = tView.Count();
            }

            return View(stat);
        }
        
    }

}