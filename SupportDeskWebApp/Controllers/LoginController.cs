using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SupportDesk;

namespace SupportDeskWebApp.Controllers
{
    public class LoginController : Controller
    {
        SupportDeskRepository dalObj = new SupportDeskRepository();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection frm)
        {
            //ViewBag.Msg=TempData["Msg"];
            
            if (frm["LoginId"] != null && frm["password"] != null)
            {
                string loginId = dalObj.ValidateCredentials(frm["LoginId"], frm["password"]);
                if (loginId != null)
                {
                    if (loginId[0] == '2')
                    {
                        //Session["Role"] = "Customer";
                        Session["LoginId"] = loginId;
                        return Redirect("/Customer/Index/");
                    }
                    else if (loginId[0] == '1')
                    {
                        //Session["Role"] = "Admin";
                        Session["LoginId"] = loginId;
                        return Redirect("/Assignee/Index/");
                    }
                    else if (loginId[0] == '4')
                    {
                        //Session["Role"] = "Staff";
                        Session["LoginId"] = loginId;
                        return Redirect("/Staff/ViewDashboard/");
                    }
                    else
                    {
                        ViewBag.Msg = "InValid Credential. Try again later.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Msg = "InValid Credential. Try again later.";
                    return View();
                }
            }
            else if (frm["UserName"] != null && frm["UserEmailId"] != null && frm["UserPassword"] != null)
            {
                string id = null; /*dalObj.AddCustomerDetail(frm["UserName"], frm["UserEmailId"], frm["UserPassword"]);*/
                if (id != null)
                {
                    Session["LoginId"] = id;
                    return Redirect("/Customer/CreateRequest/");
                }
                else
                {
                    ViewBag.Msg = "Make sure all the values are provided and email id should not be used before. Try again.";
                    return View();
                }
            }
            else
            {
                ViewBag.Msg = "InValid Data Entered.";
                return View();
            }
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}