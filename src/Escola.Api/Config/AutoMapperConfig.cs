using AutoMapper;
using Escola.Api.Config.AutoMapper;
using Escola.Domain.DTO;
using Escola.Domain.Models;

namespace Escola.Api.Config;

public static class AutoMapperConfig
{
    public static IMapper Configure()
    {
        var ConfigMap = new MapperConfiguration( config => {
         config.AddProfile(new AlunoAutoMapper());
        });

        return ConfigMap.CreateMapper();
        
    }
}
