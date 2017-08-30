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
    public class CustomerController : ApiController
    {
        SupportDeskRepository dal = new SupportDeskRepository();
        // GET: api/Customer
        [HttpGet]
        public JsonResult<List<Models.Ticket>> GetMyRequests(string custId)
        {
            SupportDeskMapper<Ticket, Models.Ticket> mapper = new SupportDeskMapper<Ticket, Models.Ticket>();
            int customerId = Convert.ToInt32(custId);
            var ticketList = dal.GetTicketsForCustomer(customerId);
            List<Models.Ticket> tickets = new List<Models.Ticket>();
            if(ticketList.Any())
            {
                foreach(var ticket in ticketList)
                {
                    tickets.Add(mapper.Translate(ticket));
                }
            }
            return Json(tickets);
        }

        // GET: api/Customer/5
        public string Get(int id)
        {
            return id.ToString();
        }

        // POST: api/Customer
        [HttpPost]
        public bool CreateRequest(Models.Ticket ticket, string custId)
        {
            var status = false;
            try
            {
                SupportDeskMapper<Models.Ticket, Ticket> mapper = new SupportDeskMapper<Models.Ticket,Ticket>();
                var ticket1 = mapper.Translate(ticket);
                var customerId = Convert.ToInt32(custId);
                status = dal.AddTicket(ticket1,customerId);

            }
            catch(System.Exception e)
            {
                status = false;
            }
            return status;
        }

        // PUT: api/Customer/5
        [HttpPut]
        public bool EditRequest(Models.Ticket ticket)
        {
            var status = false;
            try
            {
                SupportDeskMapper<Models.Ticket, Ticket> mapper = new SupportDeskMapper<Models.Ticket, Ticket>();
                
                var ticket1 = mapper.Translate(ticket);
                
                status = dal.SaveTicket(ticket1);
                
            }
            catch (System.Exception e)
            {
                status = false;
            }
            return status;
        }

        // DELETE: api/Customer/5
        [HttpDelete]
        public bool CloseRequest(int ticketId)
        {
            var status = false;
            try
            {
                //int customerId = Convert.ToInt32(ticketId);
                SupportDeskMapper<Models.Ticket, Ticket> mapper = new SupportDeskMapper<Models.Ticket, Ticket>();
                status = dal.DeleteTicket(ticketId);

            }
            catch (System.Exception e)
            {
                status = false;
            }
            return status;
        }
    }
}
