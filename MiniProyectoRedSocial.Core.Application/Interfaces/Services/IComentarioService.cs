using MiniProyectoRedSocial.Core.Application.ViewModels.Comentarios;
using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.Interfaces.Services
{
    public interface IComentarioService : IGenericService<SaveComentarioViewModel, ComentarioViewModel, Comentario>
    {
        Task<List<ComentarioViewModel>> GetComentariosByPublicacionId(int publicacionId);
        Task<ComentarioViewModel> AddSubComentario(SaveComentarioViewModel vm);
        Task<ComentarioViewModel> AddComentarioToPublicacion(SaveComentarioViewModel vm);
    }
}
