using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Domain.Entities
{
    public class Amigo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int AmigoId { get; set; }

        public Usuario Usuario { get; set; }
        public Usuario AmigoUsuario { get; set; }
    }
}
