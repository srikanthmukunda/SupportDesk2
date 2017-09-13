using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportDeskWebApi.Models
{
    public class Assignee
    {
        public int AssigneeID { get; set; }
        public string AssigneeName { get; set; }
        public string Password { get; set; }
    }
}