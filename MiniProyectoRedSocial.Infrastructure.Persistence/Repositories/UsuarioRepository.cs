using MiniProyectoRedSocial.Core.Application.Helpers;
using MiniProyectoRedSocial.Core.Application.Interfaces.Repositories;
using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
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
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationContext _dbContext;

        public UsuarioRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<Usuario> AddAsync(Usuario entity)
        {
            entity.Contraseña = PasswordInscryption.ComputeSha256Hash(entity.Contraseña);
            return await base.AddAsync(entity);
        }
        public async Task<Usuario> LoginAsync(LoginViewModel LoginVm)
        {
            string passwordEncrypty = PasswordInscryption.ComputeSha256Hash(LoginVm.Contraseña);
            Usuario usuario = await _dbContext.Set<Usuario>().FirstOrDefaultAsync(usuario => usuario.NombreUsuario == LoginVm.NombreUsuario && usuario.Contraseña == passwordEncrypty);
            return usuario;
        }

        public async Task<Usuario> GetByNombreUsuario(string nombreUsuario)
        {
            return await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<string> GetPasswordByIdAsync(int id)
        {
            return await _dbContext.Usuarios
                .Where(u => u.Id == id)
                .Select(u => u.Contraseña)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario> GetByActivationToken(string token)
        {
            return await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.ActivationToken == token);
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
            return await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

    }

}

