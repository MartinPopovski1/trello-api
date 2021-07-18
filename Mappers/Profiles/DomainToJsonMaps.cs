using AutoMapper;
using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.Mappers.Profiles
{
    public class DomainToJsonMaps : Profile
    {
        public DomainToJsonMaps()
        {
            CreateMap<Project, ProjectJson>();
            CreateMap<Board, BoardJson>();
            CreateMap<TicketList, TicketListJson>();
            CreateMap<Ticket, TicketJson>();
            CreateMap<User, UserJson>();
        }
    }
}