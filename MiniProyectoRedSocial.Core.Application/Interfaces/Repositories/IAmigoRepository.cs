using MiniProyectoRedSocial.Core.Domain.Entities;
using System.Linq.Expressions;

namespace MiniProyectoRedSocial.Core.Application.Interfaces.Repositories
{
    public interface IAmigoRepository : IGenericRepository<Amigo>
    {
        Task<List<Amigo>> GetAmigosByUsuarioId(int usuarioId);
        Task<Amigo> FirstOrDefaultAsync(Expression<Func<Amigo, bool>> predicate);
        Task<List<Amigo>> GetAllAmigos();
    }

}

