using MiniProyectoRedSocial.Core.Application.Interfaces.Repositories;
using MiniProyectoRedSocial.Core.Domain.Entities;
using MiniProyectoRedSocial.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MiniProyectoRedSocial.Infrastructure.Persistence.Repositories
{
    public class AmigoRepository : GenericRepository<Amigo>, IAmigoRepository
    {
        private readonly ApplicationContext _dbContext;

        public AmigoRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Amigo>> GetAmigosByUsuarioId(int usuarioId)
        {
            return await _dbContext.Amigos
                .Include(a => a.AmigoUsuario)
                .Include(a => a.Usuario)
                .Where(a => a.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public Task<Amigo> FirstOrDefaultAsync(Expression<Func<Amigo, bool>> predicate)
        {
            return _dbContext.Amigos.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<Amigo>> GetAllAmigos()
        {
            return await _dbContext.Amigos.ToListAsync();
        }

    }

}

