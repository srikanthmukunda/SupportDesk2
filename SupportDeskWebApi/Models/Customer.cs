using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace SupportDeskWebApi.Models
{
    public class Customer
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        public string Address { get; set; }
        [Display(Name = "Government ID")]
        public string GovernmentID { get; set; }
        public DateTime? JoinDate { get; set; }
        [Display(Name = "Mobile")]
        public string MobileNumber { get; set; }
        [Display(Name = "Secondary Mobile")]
        public string SecondaryMobileNumber { get; set; }
        [Display(Name = "Landline")]
        public string LandlineNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Email")]
        public string Email { get; set; }
        public int CustomerID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter password")]
        public string Password { get; set; }
    }
}