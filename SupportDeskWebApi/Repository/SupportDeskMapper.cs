using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SupportDesk;
using SupportDeskWebApp;


namespace SupportDeskWebApi.Repository
{
    public class SupportDeskMapper<E, M>
        where E : class
        where M : class
    {
        public SupportDeskMapper()
        {
            Mapper.CreateMap<Models.Ticket, Ticket>();
            Mapper.CreateMap<Ticket, Models.Ticket>();
            Mapper.CreateMap<Models.Customer, Customer>();
            Mapper.CreateMap<Customer, Models.Customer>();
            Mapper.CreateMap<Models.Assignee, Assignee>();
            Mapper.CreateMap<Assignee, Models.Assignee>();
        }
        public M Translate(E obj)
        {
            return Mapper.Map<E, M>(obj);
        }
    }
}