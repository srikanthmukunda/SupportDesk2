using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupportDeskWebApp.Controllers
{
    public class AssigneeController : Controller
    {
        // GET: Assignee
        public ActionResult Index()
        {
            if (Session["LoginId"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }
    }
}