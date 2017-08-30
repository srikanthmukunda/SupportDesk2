using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SupportDesk;

namespace SupportDeskWebApp.Repository
{
    public class SupportDeskMapper<E, M>
        where E : class
        where M : class
    {
        public SupportDeskMapper()
        {
            Mapper.CreateMap<Models.Ticket, Ticket>();
            Mapper.CreateMap<Ticket, Models.Ticket>();
            Mapper.CreateMap<Models.Status, Status>();
            Mapper.CreateMap<Status, Models.Status>();
            Mapper.CreateMap<Models.Application, Application>();
            Mapper.CreateMap<Application, Models.Application>();
        }
        public M Translate(E obj)
        {
            return Mapper.Map<E, M>(obj);
        }
    }

}