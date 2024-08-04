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
using MiniProyectoRedSocial.Core.Application.ViewModels.Comentarios;

namespace MiniProyectoRedSocial.Core.Application.Services
{
    public class ComentarioService : GenericService<SaveComentarioViewModel, ComentarioViewModel, Comentario>, IComentarioService
    {
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel _usuarioViewModel;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;

        public ComentarioService(IComentarioRepository comentarioRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUsuarioRepository usuarioRepository) : base(comentarioRepository, mapper)
        {
            _comentarioRepository = comentarioRepository;
            _httpContextAccessor = httpContextAccessor;
            _usuarioViewModel = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ComentarioViewModel> AddComentarioToPublicacion(SaveComentarioViewModel vm)
        {
            var usuarioViewModel = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
            var comentario = _mapper.Map<Comentario>(vm);
            comentario.UsuarioId = usuarioViewModel.Id;
            comentario.FechaHora = DateTime.Now;

            var addedComentario = await _comentarioRepository.AddAsync(comentario);

            return _mapper.Map<ComentarioViewModel>(addedComentario);
        }

        public async Task<ComentarioViewModel> AddSubComentario(SaveComentarioViewModel vm)
        {
            var usuarioViewModel = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
            var comentario = _mapper.Map<Comentario>(vm);
            comentario.UsuarioId = usuarioViewModel.Id;
            comentario.FechaHora = DateTime.Now;
            comentario.ComentarioPadreId = vm.ComentarioPadreId;

            var addedSubComentario = await _comentarioRepository.AddAsync(comentario);

            return _mapper.Map<ComentarioViewModel>(addedSubComentario);
        }

        public async Task<List<ComentarioViewModel>> GetComentariosByPublicacionId(int publicacionId)
        {
            var comentarios = await _comentarioRepository.GetComentariosByPublicacionId(publicacionId);
            var comentarioViewModels = _mapper.Map<List<ComentarioViewModel>>(comentarios);

            foreach (var comentario in comentarioViewModels)
            {
                comentario.SubComentarios = _mapper.Map<ICollection<Comentario>>(
                    comentarios.Where(c => c.ComentarioPadreId == comentario.Id).ToList()
                );
            }

            return comentarioViewModels;
        }

    }
}