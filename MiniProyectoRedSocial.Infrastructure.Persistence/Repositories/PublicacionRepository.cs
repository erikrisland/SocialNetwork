using MiniProyectoRedSocial.Core.Application.Interfaces.Repositories;
using MiniProyectoRedSocial.Core.Domain.Entities;
using MiniProyectoRedSocial.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Infrastructure.Persistence.Repositories
{
    public class PublicacionRepository : GenericRepository<Publicacion>, IPublicacionRepository
    {
        private readonly ApplicationContext _dbContext;

        public PublicacionRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Publicacion>> GetPublicacionesByUsuarioId(int usuarioId)
        {
            return await _dbContext.Publicaciones
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<List<Publicacion>> GetPublicacionesDeAmigos(List<int> amigoIds)
        {
            return await _dbContext.Set<Publicacion>()
                                 .Where(p => amigoIds.Contains(p.UsuarioId))
                                 .ToListAsync();
        }


    }

}

