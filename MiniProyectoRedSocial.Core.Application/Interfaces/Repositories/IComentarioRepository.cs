using MiniProyectoRedSocial.Core.Domain.Entities;

namespace MiniProyectoRedSocial.Core.Application.Interfaces.Repositories
{
    public interface IComentarioRepository : IGenericRepository<Comentario>
    {
        Task<List<Comentario>> GetComentariosByPublicacionId(int publicacionId);
    }

}

