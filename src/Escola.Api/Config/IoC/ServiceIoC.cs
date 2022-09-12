using Escola.Domain.Interfaces.Services;
using Escola.Domain.Services;

namespace Escola.Api.Config.IoC;

public class ServiceIoC
{
    public static void RegisterService(IServiceCollection builder)
    {
        builder.AddScoped<IAlunoServico,AlunoServico>();
        builder.AddScoped<IBoletimServico,BoletimServico>();
        builder.AddScoped<IMateriaServico,MateriaServico>();
        builder.AddScoped<INotasMateriaServico,NotasMateriaServico>();
    }
}
