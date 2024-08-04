using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios
{
    public class EditUsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe colocar su nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe colocar su apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe colocar su teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe colocar su correo")]
        public string Correo { get; set; }
        public string? FotoPerfil { get; set; }

        [Required(ErrorMessage = "Debe colocar su nombre de usuario")]
        public string NombreUsuario { get; set; }

        [DataType(DataType.Password)]
        public string? Contraseña { get; set; }

        [DataType(DataType.Password)]
        [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmarContraseña { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
        public bool Activo { get; set; }
        public string? ActivationToken { get; set; }
    }
}
