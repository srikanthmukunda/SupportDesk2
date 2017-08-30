using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace SupportDeskWebApi.Models
{
    public class Ticket
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Ticket title")]
        public string TicketTitle { get; set; }
        public int StatusID { get; set; }
        public string Assignee { get; set; }
        public int AssigneeID { get; set; }
        public string SubmitDateTime { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Ticket title")]
        public string UserQuery { get; set; }
        public int? FeatureID { get; set; }
        public string UserFullName { get; set; }
        public int CustomerID { get; set; }
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select the Application")]
        public int ApplicationID { get; set; }
        public int IncidentID { get; set; }
    }
}