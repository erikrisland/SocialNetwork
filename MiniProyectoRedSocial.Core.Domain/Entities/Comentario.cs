using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Domain.Entities
{
    public class Comentario
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
    }
}
