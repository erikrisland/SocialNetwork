using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.ViewModels.Amigos
{
    public class SaveAmigoViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int AmigoId { get; set; }

    }
}
