using MagicVilla_API.Modelos;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface INumeroVillaRepositorio : IRepositorio<NumeroVilla>
    {
        Task<NumeroVilla> Actualizar(NumeroVilla entidad);
        /*
        Task<Villa> Crear(Villa entidad);
        Task<List<Villa>> ObtenerTodos(Expression<Func<Villa, bool>> filtro = null);
        Task<Villa> Obtener(Expression<Func<Villa, bool>> filtro = null, bool tracked = true);
        Task Remover(Villa entidad);
        Task Grabar();  */
    }
}
