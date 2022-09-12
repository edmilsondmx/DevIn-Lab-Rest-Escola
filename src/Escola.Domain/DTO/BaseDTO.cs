namespace Escola.Domain.DTO;

public class BaseDTO<TEntity> where TEntity : class
{
    //envelopamento do retorno, deve ser usado como padrão ou não deverá ser utilizado
    public TEntity Data { get; set; }
    public IList<HateoasDTO> Links { get; set; }
}
