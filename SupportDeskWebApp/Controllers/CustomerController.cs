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

namespace SupportDeskWebApp.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        SupportDeskRepository dal = new SupportDeskRepository();
        public ActionResult Index()
        {
            if(Session["LoginId"]!=null)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        

        public ActionResult GetMyRequests()
        {
            if (Session["LoginId"] != null)
            {
                

                return View();
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult GetRequests()
        {
            //List<Models.Ticket> ticketList = new List<Models.Ticket>();
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                var assigneeName = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
                var application = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
                var status = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;
                SupportDeskMapper<Ticket,Models.Ticket > mapper= new SupportDeskMapper<Ticket, Models.Ticket>();
                ServiceRepository serviceObj = new ServiceRepository();
                int custID = Convert.ToInt32(Session["LoginId"]);
                var customerId = custID.ToString();
                //var lstTickets = dal.GetTicketsForCustomer(custID);
                //foreach(var ticket in lstTickets)
                //{
                //    ticketList.Add(mapper.Translate(ticket));
                //}
                HttpResponseMessage resp = serviceObj.GetResponse("api/Customer/GetMyRequests?custId=" + customerId, "JSON");
                resp.EnsureSuccessStatusCode();
                var ticketList = resp.Content.ReadAsAsync<IEnumerable<Models.Ticket>>().Result;
                if (!string.IsNullOrEmpty(assigneeName))
                {
                    ticketList = ticketList.Where(a => a.Assignee.ToLower().Contains(assigneeName.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(application))
                {
                    var applicationId = Convert.ToInt32(application);
                    ticketList = ticketList.Where(a => a.ApplicationID == applicationId).ToList();
                }
                if (!string.IsNullOrEmpty(status))
                {
                    var statusId = Convert.ToInt32(status);
                    ticketList = ticketList.Where(a => a.StatusID == statusId).ToList();
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ticketList = ticketList.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }
                totalRecords = ticketList.Count();

                var data = ticketList.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch(System.Exception e)
            {
                return null;
            }
            
        }

        public ActionResult GetAssigneeNamesForCustomer()
        {
            int custID = Convert.ToInt32(Session["LoginId"]);
            var lstTickets = dal.GetTicketsForCustomer(custID);
            var lstAssignees = dal.GetAssigneeList();
            List<string> assigneeNames = (from t in lstTickets join a in lstAssignees on t.AssigneeID equals a.AssigneeID select a.AssigneeName).ToList();
            return Json(new { data = assigneeNames }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SaveRequest(int id)
        {
            Models.Ticket ticket = new Models.Ticket();
            if (Session["LoginId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
                SupportDeskMapper<Ticket, Models.Ticket> mapper = new SupportDeskMapper<Ticket, Models.Ticket>();
                int custID = Convert.ToInt32(Session["LoginId"]);
                var ticket1 = dal.GetTicketsForCustomer(custID).Where(a=>a.IncidentID==id).FirstOrDefault();
                if(ticket1!=null)
                {
                    ticket = mapper.Translate(ticket1);
                }
                

            }
            catch(System.Exception e)
            {
                
            }
            var lstApplications = dal.GetApplicationList();
            ViewBag.ApplicationList = lstApplications;
            return View(ticket);
        }

        [HttpPost]
        public ActionResult SaveRequest(Models.Ticket ticket)
        {

            bool status = false;
            SupportDeskMapper<Models.Ticket, Ticket> mapper = new SupportDeskMapper<Models.Ticket, Ticket>();
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    int custID = Convert.ToInt32(Session["LoginId"]);
                    var customerId = custID.ToString();
                    if (ticket.IncidentID > 0)
                    {


                        var ticket2 = dal.GetTicketsForCustomer(custID).Where(a=>a.IncidentID==ticket.IncidentID).First();
                        
                        if(ticket2!=null)
                        {
                            //Ticket ticket1 = mapper.Translate(ticket);
                            //status = dal.SaveTicket(ticket1);
                            HttpResponseMessage resp = serviceObj.PutRequest("api/Customer/EditRequest", ticket, "JSON");
                            if (resp.StatusCode.ToString().Equals("OK"))
                            {
                                status = true;
                            }
                        }

                                             
                    }
                    else
                    {
                        HttpResponseMessage resp = serviceObj.PostRequest("api/Customer/CreateRequest?custId=" + customerId, ticket, "JSON");
                        //Ticket ticket1 = mapper.Translate(ticket);
                        //status = dal.AddTicket(ticket1, custID);
                        if(resp.StatusCode.ToString().Equals("OK"))
                        {
                            status = true;
                        }

                    }
                    
                    

                }
                
            }
            catch(System.Exception e)
            {
                status = false;
            }
            
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            int custID = Convert.ToInt32(Session["LoginId"]);
            var ticket = dal.GetTicketsForCustomer(custID).Where(a => a.IncidentID == id).First();
            SupportDeskMapper<Ticket, Models.Ticket> mapper = new SupportDeskMapper<Ticket, Models.Ticket>();
            SupportDeskMapper<Status, Models.Status> sMapper = new SupportDeskMapper<Status, Models.Status>();
            SupportDeskMapper<Application, Models.Application> aMapper = new SupportDeskMapper<Application, Models.Application>();
            if (ticket!=null)
            {
                var ticket1 = mapper.Translate(ticket);
                var status = dal.GetStatusList().Where(a => a.StatusID == ticket.StatusID).First();
                var mStatus = sMapper.Translate(status);
                var application = dal.GetApplicationList().Where(a => a.ApplicationId == ticket.ApplicationID).First();
                var mApplication = aMapper.Translate(application);
                ViewBag.Status = mStatus.Status1;
                ViewBag.Application = mApplication.Application1;
                return View(ticket1);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteTicket(int id)
        {
            bool status = false;
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                int custID = Convert.ToInt32(Session["LoginId"]);
                //string customerId = custID.ToString();
                var ticket = dal.GetTicketsForCustomer(custID).Where(a => a.IncidentID == id).First();
                if(ticket!=null)
                {
                    HttpResponseMessage resp = serviceObj.DeleteRequest("api/Customer/CloseRequest?ticketId=" + id, "JSON");
                    if (resp.StatusCode.ToString().Equals("OK"))
                    {
                        status = true;
                    }
                    //status = dal.DeleteTicket(id);
                }
                
            }
            catch(System.Exception e)
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}