using GuardianAngels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuardianAngels.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Home()
        {
            ViewBag.IsHome = true;
            return View();
        }

        public ActionResult Admin()
        {
            ViewBag.IsHome = true;
            return View();
        }


        public ActionResult AdminMessages()
        {
            return View();
        }

        public ActionResult DriverMenu()
        {
            return View();
        }
        
    }
}