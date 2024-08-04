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
using MiniProyectoRedSocial.Core.Application.ViewModels.Publicaciones;

namespace MiniProyectoRedSocial.Core.Application.Services
{
    public class PublicacionService : GenericService<SavePublicacionViewModel, PublicacionViewModel, Publicacion>, IPublicacionService
    {
        private readonly IPublicacionRepository _publicacionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel _usuarioViewModel;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IComentarioService _comentarioService;

        public PublicacionService(IPublicacionRepository publicacionRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUsuarioRepository usuarioRepository, IComentarioService comentarioService) : base(publicacionRepository, mapper)
        {
            _publicacionRepository = publicacionRepository;
            _httpContextAccessor = httpContextAccessor;
            _usuarioViewModel = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _comentarioService = comentarioService;
        }

        public override async Task<SavePublicacionViewModel> Add(SavePublicacionViewModel vm)
        {
            var publicacion = _mapper.Map<Publicacion>(vm);

            publicacion.UsuarioId = _usuarioViewModel.Id;
            publicacion.FechaHora = DateTime.Now;

            var addedPublicacion = await _publicacionRepository.AddAsync(publicacion);

            return _mapper.Map<SavePublicacionViewModel>(addedPublicacion);
        }

        public override async Task<List<PublicacionViewModel>> GetAllViewModel()
        {
            var entityList = await _publicacionRepository.GetAllAsync();
            var publicationViewModels = _mapper.Map<List<PublicacionViewModel>>(entityList);

            foreach (var publication in publicationViewModels)
            {
                var usuario = await _usuarioRepository.GetByIdAsync(publication.UsuarioId);
                publication.UsuarioNombre = usuario.Nombre;
                publication.UsuarioApellido = usuario.Apellido;
                publication.UsuarioImagenPerfil = usuario.FotoPerfil;

                publication.Comentarios = await _comentarioService.GetComentariosByPublicacionId(publication.Id);
            }

            return publicationViewModels;
        }

        public async Task<List<PublicacionViewModel>> GetPublicacionesByUsuarioId(int usuarioId)
        {
            var entityList = await _publicacionRepository.GetPublicacionesByUsuarioId(usuarioId);
            var publicationViewModels = _mapper.Map<List<PublicacionViewModel>>(entityList);

            foreach (var publication in publicationViewModels)
            {
                var usuario = await _usuarioRepository.GetByIdAsync(publication.UsuarioId);
                publication.UsuarioNombre = usuario.Nombre;
                publication.UsuarioApellido = usuario.Apellido;
                publication.UsuarioImagenPerfil = usuario.FotoPerfil;

                publication.Comentarios = await _comentarioService.GetComentariosByPublicacionId(publication.Id);
            }

            return publicationViewModels;
        }

        public async Task<List<PublicacionViewModel>> GetPublicacionesDeAmigos(List<int> amigoIds)
        {
            var entityList = await _publicacionRepository.GetPublicacionesDeAmigos(amigoIds);
            var publicationViewModels = _mapper.Map<List<PublicacionViewModel>>(entityList);

            publicationViewModels = publicationViewModels.Where(p => p.UsuarioId != _usuarioViewModel.Id).ToList();

            foreach (var publication in publicationViewModels)
            {
                var usuario = await _usuarioRepository.GetByIdAsync(publication.UsuarioId);
                publication.UsuarioNombre = usuario.Nombre;
                publication.UsuarioApellido = usuario.Apellido;
                publication.UsuarioImagenPerfil = usuario.FotoPerfil;

                publication.Comentarios = await _comentarioService.GetComentariosByPublicacionId(publication.Id);
            }

            return publicationViewModels;
        }

    }
}