using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProyectoRedSocial.Core.Application.Interfaces.Services;
using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
using MiniProyectoRedSocial.Core.Application.Helpers;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using MiniProyectoRedSocial.Middlewares;

namespace MiniProyectoRedSocial.Controllers
{
    public class PerfilController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly ValidateUserSession _validateUserSession;

        public PerfilController(IUsuarioService usuarioService, IMapper mapper, ValidateUserSession validateUserSession)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            UsuarioViewModel usuario = HttpContext.Session.Get<UsuarioViewModel>("usuario");

            if (usuario == null)
            {
                return RedirectToAction("Index", "Login");
            }

            EditUsuarioViewModel vm = await _usuarioService.GetByIdEditViewModel(usuario.Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditUsuarioViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var usuarioActual = await _usuarioService.GetByIdEditViewModel(vm.Id);

            if (usuarioActual.NombreUsuario != vm.NombreUsuario)
            {
                var existingUser = await _usuarioService.GetByNombreUsuario(vm.NombreUsuario);
                if (existingUser != null && existingUser.Id != vm.Id)
                {
                    ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya está en uso.");
                    return View(vm);
                }
            }

            var saveViewModel = _mapper.Map<SaveUsuarioViewModel>(vm);

            if (string.IsNullOrEmpty(vm.Contraseña))
            {
                saveViewModel.Contraseña = usuarioActual.Contraseña;
            }
            else
            {
                saveViewModel.Contraseña = PasswordInscryption.ComputeSha256Hash(vm.Contraseña);
            }

            if (vm.File != null)
            {
                saveViewModel.FotoPerfil = UploadFile(vm.File, vm.Id);
            }
            else
            {
                saveViewModel.FotoPerfil = usuarioActual.FotoPerfil;
            }

            await _usuarioService.Update(saveViewModel, saveViewModel.Id);

            return RedirectToAction("Index", "Home");
        }
        private string UploadFile(IFormFile file, int id)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            string basePath = $"/Images/FotoUsuario/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new FileInfo(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"{basePath}/{fileName}";
        }
    }
}