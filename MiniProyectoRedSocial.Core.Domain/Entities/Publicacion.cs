using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Domain.Entities
{
    public class Publicacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Imagen { get; set; }
        public string? VideoUrl { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<Comentario> Comentarios { get; set; }
    }
}
