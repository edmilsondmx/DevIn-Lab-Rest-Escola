using AutoMapper;
using Escola.Domain.DTO;
using Escola.Domain.Models;

namespace Escola.Api.Config.AutoMapper;

public class AlunoAutoMapper : Profile
{
    public AlunoAutoMapper()
    {
        CreateMap<AlunoDTO, AlunoV2DTO>()
            .ForMember(s => s.RA, o => o.MapFrom(d => d.Matricula))
            .ReverseMap();
        CreateMap<AlunoDTO, Aluno>()
            .ReverseMap();
    }
}
