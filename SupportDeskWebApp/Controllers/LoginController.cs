using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SupportDesk;
using SupportDeskWebApp.Repository;

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
                        
                        Session["LoginId"] = loginId;
                        return Redirect("/Customer/Index/");
                    }
                    else if (loginId[0] == '1')
                    {
                        
                        Session["LoginId"] = loginId;
                        return Redirect("/Assignee/Index/");
                    }
                    else if (loginId[0] == '4')
                    {
                       
                        Session["LoginId"] = loginId;
                        return Redirect("/Staff/ViewDashboard/");
                    }
                    else if (loginId[0] == '6')
                    {                       
                        Session["LoginId"] = loginId;
                        return Redirect("/Admin/Index/");
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
                
                Models.Customer customer = new Models.Customer();
                customer.FirstName = frm["UserName"].ToString();
                customer.MiddleName= frm["MiddleName"].ToString();
                customer.LastName = frm["LastName"].ToString();
                customer.Gender= frm["Gender"].ToString();
                customer.GovernmentID = frm["GovernmentID"].ToString();
                customer.JoinDate = DateTime.Now;
                customer.MobileNumber = frm["Mobile"].ToString();
                customer.SecondaryMobileNumber = frm["Mobile2"].ToString();
                customer.LandlineNumber = frm["Landline"].ToString();
                customer.Email = frm["UserEmailId"].ToString();
                customer.Password = frm["UserPassword"].ToString();
                SupportDeskMapper<Models.Customer, Customer> mapper = new SupportDeskMapper<Models.Customer, Customer>();
                var newCustomer = mapper.Translate(customer);
                string id = dalObj.RegisterCustomer(newCustomer).ToString();
                if (id != null)
                {
                    Session["LoginId"] = id;
                    return Redirect("/Customer/Index/");
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
        [HttpPost]
        public bool CheckValidEmailId(string EmailId)
        {            
            return dalObj.CheckValidEmailId(EmailId);
        }
        
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}