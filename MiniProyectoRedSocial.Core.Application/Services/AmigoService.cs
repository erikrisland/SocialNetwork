using MiniProyectoRedSocial.Core.Application.Helpers;
using MiniProyectoRedSocial.Core.Application.Interfaces.Repositories;
using MiniProyectoRedSocial.Core.Application.Interfaces.Services;
using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
using MiniProyectoRedSocial.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MiniProyectoRedSocial.Core.Application.ViewModels.Amigos;
using MiniProyectoRedSocial.Core.Application.ViewModels.Publicaciones;

namespace MiniProyectoRedSocial.Core.Application.Services
{
    public class AmigoService : GenericService<SaveAmigoViewModel, AmigoViewModel, Amigo>, IAmigoService
    {
        private readonly IAmigoRepository _amigoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel _usuarioViewModel;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPublicacionRepository _publicacionRepository;

        public AmigoService(IAmigoRepository amigoRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUsuarioRepository usuarioRepository, IPublicacionRepository publicacionRepository) : base(amigoRepository, mapper)
        {
            _amigoRepository = amigoRepository;
            _httpContextAccessor = httpContextAccessor;
            _usuarioViewModel = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _publicacionRepository = publicacionRepository;
        }

        public async Task<List<AmigoViewModel>> GetAmigosByUsuarioId(int usuarioId)
        {
            var amigos = await _amigoRepository.GetAmigosByUsuarioId(usuarioId);
            return _mapper.Map<List<AmigoViewModel>>(amigos);
        }

        public async Task<bool> AddAmigo(int usuarioId, int amigoId)
        {
            var amigo = new Amigo { UsuarioId = usuarioId, AmigoId = amigoId };
            await _amigoRepository.AddAsync(amigo);
            return true;
        }

        public override async Task<List<AmigoViewModel>> GetAllViewModel()
        {
            var usuarioActual = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");

            if (usuarioActual == null)
            {
                return new List<AmigoViewModel>();
            }

            var amigos = await _amigoRepository.GetAmigosByUsuarioId(usuarioActual.Id);

            var publicaciones = await _publicacionRepository.GetAllAsync();

            var amigosViewModel = amigos.Select(amigo => new AmigoViewModel
            {
                Id = amigo.Id,
                UsuarioId = amigo.UsuarioId,
                AmigoId = amigo.AmigoId,
                Nombre = amigo.AmigoUsuario.Nombre,
                Apellido = amigo.AmigoUsuario.Apellido,
                NombreUsuario = amigo.AmigoUsuario.NombreUsuario,
                FotoPerfil = amigo.AmigoUsuario.FotoPerfil,
                Usuario = new UsuarioViewModel
                {
                    Id = amigo.Usuario.Id,
                    Nombre = amigo.Usuario.Nombre,
                    Apellido = amigo.Usuario.Apellido,
                    NombreUsuario = amigo.Usuario.NombreUsuario,
                    FotoPerfil = amigo.Usuario.FotoPerfil
                },
                AmigoUsuario = new UsuarioViewModel
                {
                    Id = amigo.AmigoUsuario.Id,
                    Nombre = amigo.AmigoUsuario.Nombre,
                    Apellido = amigo.AmigoUsuario.Apellido,
                    NombreUsuario = amigo.AmigoUsuario.NombreUsuario,
                    FotoPerfil = amigo.AmigoUsuario.FotoPerfil
                },
                Publicaciones = publicaciones
                    .Where(p => p.UsuarioId == amigo.AmigoId)
                    .Select(p => new PublicacionViewModel
                    {
                        Id = p.Id,
                        UsuarioId = p.UsuarioId,
                        Contenido = p.Contenido,
                        FechaHora = p.FechaHora,
                        Imagen = p.Imagen,
                        VideoUrl = p.VideoUrl
                    }).ToList()
            }).ToList();

            return amigosViewModel;
        }

        public async Task<List<AmigoViewModel>> GetAmigosDeUsuarioActual()
        {
            var usuario = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
            if (usuario == null)
            {
                return new List<AmigoViewModel>();
            }

            var amigos = await _amigoRepository.GetAmigosByUsuarioId(usuario.Id);
            return _mapper.Map<List<AmigoViewModel>>(amigos);
        }

    }
}