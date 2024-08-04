using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.Interfaces.Services
{
    public interface IUsuarioService : IGenericService<SaveUsuarioViewModel, UsuarioViewModel, Usuario>
    {
        Task<UsuarioViewModel> Login(LoginViewModel loginVm);
        Task<UsuarioViewModel> GetByActivationToken(string token);
        Task<UsuarioViewModel> GetByNombreUsuario(string nombreUsuario);
        Task<EditUsuarioViewModel> GetByIdEditViewModel(int id);
    }
}
