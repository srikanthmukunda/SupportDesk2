//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SupportDesk
{
    using System;
    using System.Collections.Generic;
    
    public partial class TicketComment
    {
        public int TicketCommentId { get; set; }
        public int TicketId { get; set; }
        public string PerformedBy { get; set; }
        public int Action { get; set; }
        public string ActionDate { get; set; }
        public string TimeSpent { get; set; }
        public string TicketComment1 { get; set; }
    
        public virtual Ticket Ticket { get; set; }
    }
}