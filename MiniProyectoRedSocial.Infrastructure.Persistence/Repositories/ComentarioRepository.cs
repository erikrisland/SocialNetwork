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
    public class ComentarioRepository : GenericRepository<Comentario>, IComentarioRepository
    {
        private readonly ApplicationContext _dbContext;

        public ComentarioRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Comentario>> GetComentariosByPublicacionId(int publicacionId)
        {
            return await _dbContext.Comentarios
                                   .Where(c => c.PublicacionId == publicacionId)
                                   .Include(c => c.Usuario)
                                   .ToListAsync();
        }

    }

}

