using MiniProyectoRedSocial.Core.Application.ViewModels.Comentarios;
using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.ViewModels.Publicaciones
{
    public class PublicacionViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Imagen { get; set; }
        public string VideoUrl { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<ComentarioViewModel> Comentarios { get; set; }

        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioImagenPerfil { get; set; }
    }
}
