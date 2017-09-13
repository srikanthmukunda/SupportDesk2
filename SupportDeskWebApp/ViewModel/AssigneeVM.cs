using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportDeskWebApp.ViewModel
{
    public class AssigneeVM
    {
        public Models.Assignee Assignee { get; set; }
        public List<Models.Ticket> TicketList { get; set; }
    }
}