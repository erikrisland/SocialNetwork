using MiniProyectoRedSocial.Core.Domain.Entities;

namespace MiniProyectoRedSocial.Core.Application.Interfaces.Repositories
{
    public interface IPublicacionRepository : IGenericRepository<Publicacion>
    {
        Task<List<Publicacion>> GetPublicacionesByUsuarioId(int usuarioId);
        Task<List<Publicacion>> GetPublicacionesDeAmigos(List<int> amigoIds);
    }

}

