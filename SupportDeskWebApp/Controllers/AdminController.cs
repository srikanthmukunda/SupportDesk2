using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SupportDesk;
using SupportDeskWebApp.Repository;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Net.Http.Formatting;
using SupportDeskWebApp.ViewModel;

namespace SupportDeskWebApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["LoginId"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult GetAllCustomers()
        {
            if (Session["LoginId"] != null)
            {
                try
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage resp = serviceObj.GetResponse("api/Admin/GetAllCustomers", "JSON");
                    resp.EnsureSuccessStatusCode();
                    var customerList = resp.Content.ReadAsAsync<IEnumerable<Models.Customer>>().Result;
                    customerList = customerList.OrderBy(a => a.CustomerID);
                    return View(customerList);
                }
                catch(System.Exception e)
                {

                }
                
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult GetAssigneeTickets()
        {
            List<AssigneeVM> assigneeTickets = new List<AssigneeVM>();
            if (Session["LoginId"] != null)
            {
                try
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage resp = serviceObj.GetResponse("api/Admin/GetAllAssignees", "JSON");
                    resp.EnsureSuccessStatusCode();
                    var assigneeList = resp.Content.ReadAsAsync<IEnumerable<Models.Assignee>>().Result;
                    foreach(var assignee in assigneeList)
                    {
                        HttpResponseMessage resp1 = serviceObj.GetResponse("api/Admin/GetTicketsForAssignee?assigneeId="+assignee.AssigneeID, "JSON");
                        resp1.EnsureSuccessStatusCode();
                        var ticketList = resp1.Content.ReadAsAsync<IEnumerable<Models.Ticket>>().Result.ToList();
                        assigneeTickets.Add(new AssigneeVM { Assignee = assignee, TicketList = ticketList });
                    }
                    return View(assigneeTickets);
                }
                catch (System.Exception e)
                {

                }
            }
            return RedirectToAction("Index", "Login");

        }



    }
}