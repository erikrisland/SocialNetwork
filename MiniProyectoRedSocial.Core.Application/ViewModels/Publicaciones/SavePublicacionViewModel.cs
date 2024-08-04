using Microsoft.AspNetCore.Http;
using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.ViewModels.Publicaciones
{
    public class SavePublicacionViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Debe colocar una descripción.")]
        public string Contenido { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Imagen { get; set; }
        public string? VideoUrl { get; set; }

        public IFormFile? File { get; set; }

    }
}
