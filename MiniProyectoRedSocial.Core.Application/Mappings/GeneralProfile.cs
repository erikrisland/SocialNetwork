using AutoMapper;
using MiniProyectoRedSocial.Core.Application.ViewModels.Amigos;
using MiniProyectoRedSocial.Core.Application.ViewModels.Comentarios;
using MiniProyectoRedSocial.Core.Application.ViewModels.Publicaciones;
using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile() 
        {
            CreateMap<EditUsuarioViewModel, SaveUsuarioViewModel>();

            CreateMap<SaveUsuarioViewModel, Usuario>();

            CreateMap<Usuario, UsuarioViewModel>();

            CreateMap<Usuario, SaveUsuarioViewModel>()
                .ForMember(dest => dest.ConfirmarContraseña, opt => opt.Ignore())
                .ForMember(dest => dest.File, opt => opt.Ignore())
                .ForMember(dest => dest.ActivationToken, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Publicaciones, opt => opt.Ignore())
                .ForMember(dest => dest.Amigos, opt => opt.Ignore());

            CreateMap<Usuario, EditUsuarioViewModel>();

            CreateMap<Amigo, AmigoViewModel>();

            CreateMap<Amigo, SaveAmigoViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.AmigoUsuario, opt => opt.Ignore());

            CreateMap<Comentario, ComentarioViewModel>();

            CreateMap<Comentario, SaveComentarioViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Publicacion, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.SubComentarios, opt => opt.Ignore());

            CreateMap<Publicacion, PublicacionViewModel>();

            CreateMap<Publicacion, SavePublicacionViewModel>()
                .ForMember(dest => dest.File, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Comentarios, opt => opt.Ignore());
        }
    }
}
