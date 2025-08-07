using AutoMapper;
using Eventia.Application.DTOs.TicketEvento;
using Eventia.Domain.Entities;

namespace Eventia.Application.MappingProfiles
{
    public class TicketEventoProfile : Profile
    {
        public TicketEventoProfile()
        {
            CreateMap<TicketEvento, TicketEventoDto>()
                .ForMember(dest => dest.NombreUsuarioResponsable,
                           opt => opt.MapFrom(src => src.UsuarioResponsable.Nombre));
        }
    }
}
