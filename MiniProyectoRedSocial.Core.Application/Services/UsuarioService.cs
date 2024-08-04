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
using MiniProyectoRedSocial.Core.Application.Dtos.Email;

namespace MiniProyectoRedSocial.Core.Application.Services
{
    public class UsuarioService : GenericService<SaveUsuarioViewModel, UsuarioViewModel, Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel _usuarioViewModel;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IEmailService emailService) : base(usuarioRepository, mapper)
        {
            _usuarioRepository = usuarioRepository;
            _httpContextAccessor = httpContextAccessor;
            _usuarioViewModel = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<UsuarioViewModel> Login(LoginViewModel loginVm)
        {
            Usuario usuario = await _usuarioRepository.LoginAsync(loginVm);

            if (usuario == null)
            {
                return null;
            }

            UsuarioViewModel usuarioVm = _mapper.Map<UsuarioViewModel>(usuario);

            return usuarioVm;
        }

        public override async Task<SaveUsuarioViewModel> Add(SaveUsuarioViewModel vm)
        {
            vm.Activo = false;
            string token = Guid.NewGuid().ToString();
            Usuario usuario = _mapper.Map<Usuario>(vm);
            usuario.ActivationToken = token;

            var usuarioAgregado = await _usuarioRepository.AddAsync(usuario);

            string activationLink = $"https://{_httpContextAccessor.HttpContext.Request.Host}/Login/ActivateAccount?token={token}";

            await _emailService.SendAsync(new EmailRequest
            {
                To = usuario.Correo,
                Subject = "Activa tu cuenta",
                Body = $"<h2>Bienvenido a Facebook 2, {usuarioAgregado.Nombre} {usuarioAgregado.Apellido}</h2><h4>Tu usuario es: {usuarioAgregado.NombreUsuario}</h4><p>Para activar tu cuenta, haz clic en el siguiente enlace: <a href='{activationLink}'>Activar cuenta</a></p>"
            });

            return _mapper.Map<SaveUsuarioViewModel>(usuarioAgregado);
        }

        public async Task<UsuarioViewModel> GetByActivationToken(string token)
        {
            var user = await _usuarioRepository.GetByActivationToken(token);
            return _mapper.Map<UsuarioViewModel>(user);
        }

        public async Task<UsuarioViewModel> GetByNombreUsuario(string nombreUsuario)
        {
            var usuario = await _usuarioRepository.GetByNombreUsuario(nombreUsuario);
            if (usuario == null) return null;

            return new UsuarioViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                FotoPerfil= usuario.FotoPerfil,
                NombreUsuario = usuario.NombreUsuario,
                Activo= usuario.Activo,
                ActivationToken= usuario.ActivationToken,
            };
        }

        public async Task<EditUsuarioViewModel> GetByIdEditViewModel(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);

            if (usuario == null)
            {
                return null;
            }

            return _mapper.Map<EditUsuarioViewModel>(usuario);
        }

    }
}