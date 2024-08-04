using MiniProyectoRedSocial.Core.Application.ViewModels.Amigos;
using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.Interfaces.Services
{
    public interface IAmigoService : IGenericService<SaveAmigoViewModel, AmigoViewModel, Amigo>
    {
        Task<List<AmigoViewModel>> GetAmigosByUsuarioId(int usuarioId);
        Task<bool> AddAmigo(int usuarioId, int amigoId);
        Task<List<AmigoViewModel>> GetAmigosDeUsuarioActual();
    }
}
