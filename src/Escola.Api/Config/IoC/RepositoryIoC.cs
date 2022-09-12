using Escola.Domain.Interfaces.Repositories;
using Escola.Infra.DataBase.Repositories;

namespace Escola.Api.Config.IoC;

public class RepositoryIoC
{
    public static void RegisterService(IServiceCollection builder)
    {
        builder.AddScoped<IAlunoRepositorio,AlunoRepositorio>();
        builder.AddScoped<IBoletimRepositorio,BoletimRepositorio>();
        builder.AddScoped<IMateriaRepositorio,MateriaRepositorio>();
        builder.AddScoped<INotasMateriaRepositorio,NotasMateriaRepositorio>();
    }
}
