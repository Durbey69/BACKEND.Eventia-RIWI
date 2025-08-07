using AutoMapper;
using Eventia.Application.DTOs.Usuario;
using Eventia.Domain.Entities;

namespace Eventia.API.Mappings;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioDto>().ReverseMap();
        CreateMap<Usuario, CreateUsuarioDto>().ReverseMap();
        CreateMap<Usuario, UpdateUsuarioDto>().ReverseMap();
    }
}
