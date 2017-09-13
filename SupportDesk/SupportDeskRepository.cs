using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SupportDesk
{
    public class SupportDeskRepository
    {
        SupportDeskDBContext Context;
        public SupportDeskRepository()
        {
            Context = new SupportDeskDBContext();
        }

        public string ValidateCredentials(string id, string password)
        {
            int result = 0;
            string loginId = id.ToString(); 

            if (loginId[0] == '1')
            {
                Assignee assignee = FetchAssigneeDetail(Convert.ToInt32(id));
                if (assignee != null && assignee.Password == password)
                {
                    result = assignee.AssigneeID;
                }
            }
            else if (loginId[0] == '4')
            {
                Agent agent = FetchAgentDetail(Convert.ToInt32(id));
                if (agent != null && agent.Password == password)
                {
                    result = agent.AgentId;
                }
            }
            else if (loginId[0] == '6')
            {
                Admin admin = FetchAdminDetail(Convert.ToInt32(id));
                if (admin != null && admin.AdminPassword == password)
                {
                    result = admin.AdminID;
                }
            }
            else
            {
                Customer cd = FetchCustomerDetail(id);
                if (cd != null && cd.Password == password)
                {
                    result = cd.CustomerID;
                }
            }
            return result.ToString();
        }
        public Admin FetchAdminDetail(int adminId)
        {
            Admin result;
            try
            {
                result = Context.Admins.Where(x => x.AdminID == adminId).First<Admin>();
            }
            catch (System.Exception e)
            {
                result = null;
            }
            return result;
        }
        public Assignee FetchAssigneeDetail(int assigneeId)
        {
            Assignee result;
            try
            {
                result = Context.Assignees.Where(x => x.AssigneeID == assigneeId).First<Assignee>();
            }
            catch (System.Exception e)
            {
                result = null;
            }
            return result;
        }

        public Agent FetchAgentDetail(int Id)
        {
            Agent result;
            try
            {
                result = Context.Agents.Where(x => x.AgentId == Id).First<Agent>();
            }
            catch (System.Exception e)
            {
                result = null;
            }
            return result;
        }

        public Customer FetchCustomerDetail(string customerId)
        {
            Customer cd;
            try
            {
                int custID = 0;
                if(customerId[0]=='2')
                {
                    custID = Convert.ToInt32(customerId);
                }
                cd = Context.Customers.Where(x => x.CustomerID == custID || x.Email == customerId).First();
            }
            catch (System.Exception e)
            {
                cd = null;
            }
            return cd;
        }

        public List<Ticket> GetTicketsForCustomer(int customerID)
        {
            List<Ticket> ticketList = new List<Ticket>();
            ticketList = Context.Tickets.Where(a => a.CustomerID == customerID).ToList();
            return ticketList;
        }

        public List<Ticket> GetTicketsForAssignee(int assigneeId)
        {
            var ticketList = Context.Tickets.Where(a => a.AssigneeID == assigneeId).ToList();
            return ticketList;
        }

        public List<Application> GetApplicationList()
        {
            List<Application> applicationList = new List<Application>();
            applicationList = Context.Applications.ToList();
            return applicationList;
        }

        public List<Status> GetStatusList()
        {
            return Context.Status.ToList();
        }
        public List<Customer> GetCustomerList()
        {
            return Context.Customers.ToList();
        }

        public List<Assignee> GetAssigneeList()
        {
            return Context.Assignees.ToList();
        }

        public bool SaveTicket(Ticket ticket)
        {
            bool status = false;
            try
            {
                var ticket1 = Context.Tickets.Where(a => a.IncidentID == ticket.IncidentID).First();
                if(ticket1!=null)
                {
                    ticket1.ApplicationID = ticket.ApplicationID;
                    ticket1.TicketTitle = ticket.TicketTitle;
                    ticket1.UserQuery = ticket.UserQuery;
                    ticket1.FeatureID = ticket.FeatureID;
                    Context.SaveChanges();
                    status = true;
                }
            }
            catch(System.Exception e)
            {
                status = false;
            }
            return status;
        }

        public bool AddTicket(Ticket ticket, int custId)
        {
            bool status = false;
            try
            {
                var customer = Context.Customers.Where(a => a.CustomerID == custId).First();
                ticket.StatusID = 1;
                ticket.SubmitDateTime = DateTime.Now.ToString();
                ticket.UserFullName = customer.FirstName + " " + customer.MiddleName + " " + customer.LastName;
                ticket.CustomerID = custId;
                ticket.Email = customer.Email;
                ticket.AssigneeID = 1;
                ticket.Assignee = Context.Assignees.Where(p=>p.AssigneeID==1).Select(a => a.AssigneeName).First();
                Context.Tickets.Add(ticket);
                Context.SaveChanges();
                status = true;
            }
            catch (System.Exception e)
            {
                status = false;
            }
            return status;
        }

        public bool DeleteTicket(int  id)
        {
            bool status = false;
            try
            {
                var ticket = Context.Tickets.Where(a => a.IncidentID == id).First();
                ticket.StatusID = 5;
                Context.SaveChanges();
                status = true;
            }
            catch (System.Exception e)
            {
                status = false;
            }
            return status;
        }

        public int RegisterCustomer(Customer customer)
        {
            Context.Customers.Add(customer);
            Context.SaveChanges();
            int id = Context.Customers.Where(a => a.Email == customer.Email).Select(a => a.CustomerID).First();
            return id;
        }

        public bool CheckValidEmailId(string EmailId)
        {
            bool result;
            try
            {
                Customer cd = Context.Customers.Where(x => x.Email == EmailId).FirstOrDefault();
                if (cd == null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (System.Exception)
            {
                result = false;
            }
            return result;
        }


    }
}
