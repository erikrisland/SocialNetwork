using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.ViewModels.Comentarios
{
    public class ComentarioViewModel
    {
        public int Id { get; set; }
        public int PublicacionId { get; set; }
        public int UsuarioId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaHora { get; set; }
        public int? ComentarioPadreId { get; set; }

        public Publicacion Publicacion { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<Comentario> SubComentarios { get; set; }

        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioImagenPerfil { get; set; }
    }
}
