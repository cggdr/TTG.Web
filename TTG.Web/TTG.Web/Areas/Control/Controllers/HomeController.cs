using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTG.Web.Areas.Control.Controllers
{
    public class HomeController : Controller
    {
        // GET: Control/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}