using MagicVilla_API.Modelos;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IVillaRepositorio : IRepositorio<Villa>
    {
        Task<Villa> Actualizar(Villa entidad);
        /*
        Task<Villa> Crear(Villa entidad);
        Task<List<Villa>> ObtenerTodos(Expression<Func<Villa, bool>> filtro = null);
        Task<Villa> Obtener(Expression<Func<Villa, bool>> filtro = null, bool tracked = true);
        Task Remover(Villa entidad);
        Task Grabar();  */
    }
}
