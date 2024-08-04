using MiniProyectoRedSocial.Core.Application.ViewModels.Publicaciones;
using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.ViewModels.Amigos
{
    public class AmigoViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int AmigoId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string FotoPerfil { get; set; }

        public UsuarioViewModel Usuario { get; set; }
        public UsuarioViewModel AmigoUsuario { get; set; }

        public List<PublicacionViewModel> Publicaciones { get; set; }

    }
}
