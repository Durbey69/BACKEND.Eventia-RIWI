using AutoMapper;
using Eventia.Application.DTOs.Ticket;
using Eventia.Domain.Entities;

namespace Eventia.API.Mappings;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        CreateMap<Ticket, TicketDto>().ReverseMap();
        CreateMap<Ticket, CreateTicketDto>().ReverseMap();
        CreateMap<Ticket, UpdateTicketDto>().ReverseMap();
    }
}
