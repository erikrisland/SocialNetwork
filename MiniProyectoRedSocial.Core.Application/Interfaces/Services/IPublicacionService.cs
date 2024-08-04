using MiniProyectoRedSocial.Core.Application.ViewModels.Publicaciones;
using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.Interfaces.Services
{
    public interface IPublicacionService : IGenericService<SavePublicacionViewModel, PublicacionViewModel, Publicacion>
    {
        Task<List<PublicacionViewModel>> GetPublicacionesByUsuarioId(int usuarioId);
        Task<List<PublicacionViewModel>> GetPublicacionesDeAmigos(List<int> amigoIds);
    }
}
