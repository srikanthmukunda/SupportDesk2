using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using SupportDesk;
using SupportDeskWebApi.Repository;

namespace SupportDeskWebApi.Controllers
{
    public class AdminController : ApiController
    {
        SupportDeskRepository dal = new SupportDeskRepository();
        // GET: api/Admin
        [HttpGet]
        public JsonResult<List<Models.Customer>> GetAllCustomers()
        {
            SupportDeskMapper<Customer, Models.Customer> mapper = new SupportDeskMapper<Customer, Models.Customer>();
            
            var customerList = dal.GetCustomerList();
            List<Models.Customer> customers = new List<Models.Customer>();
            if (customerList.Any())
            {
                foreach (var customer in customerList)
                {
                    customers.Add(mapper.Translate(customer));
                }
            }
            return Json(customers);
        }

        [HttpGet]
        public JsonResult<List<Models.Assignee>> GetAllAssignees()
        {
            SupportDeskMapper<Assignee, Models.Assignee> mapper = new SupportDeskMapper<Assignee, Models.Assignee>();

            var assigneeList = dal.GetAssigneeList().OrderByDescending(a=>a.AssigneeID);
            List<Models.Assignee> assignees = new List<Models.Assignee>();
            if (assigneeList.Any())
            {
                foreach (var assignee in assigneeList)
                {
                    assignees.Add(mapper.Translate(assignee));
                }
            }
            return Json(assignees);
        }
        [HttpGet]
        public JsonResult<List<Models.Ticket>> GetTicketsForAssignee(int assigneeId)
        {
            SupportDeskMapper<Ticket, Models.Ticket> mapper = new SupportDeskMapper<Ticket, Models.Ticket>();

            var ticketList = dal.GetTicketsForAssignee(assigneeId);
            List<Models.Ticket> tickets = new List<Models.Ticket>();
            if (ticketList.Any())
            {
                foreach (var ticket in ticketList)
                {
                    tickets.Add(mapper.Translate(ticket));
                }
            }
            return Json(tickets);
        }

        // GET: api/Admin/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Admin
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Admin/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Admin/5
        public void Delete(int id)
        {
        }
    }
}
