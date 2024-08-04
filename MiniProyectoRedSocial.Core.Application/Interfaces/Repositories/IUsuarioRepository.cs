using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
using MiniProyectoRedSocial.Core.Domain.Entities;

namespace MiniProyectoRedSocial.Core.Application.Interfaces.Repositories
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> LoginAsync(LoginViewModel LoginVm);
        Task<Usuario> GetByActivationToken(string token);
        Task<Usuario> GetByNombreUsuario(string nombreUsuario);
        Task<Usuario> GetUsuarioById(int usuarioId);

    }

}

