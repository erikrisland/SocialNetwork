using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProyectoRedSocial.Core.Application.Interfaces.Services;
using MiniProyectoRedSocial.Core.Application.Services;
using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
using MiniProyectoRedSocial.Core.Application.Helpers;
using Newtonsoft.Json;
using MiniProyectoRedSocial.Middlewares;
using MiniProyectoRedSocial.Core.Application.ViewModels.Publicaciones;
using MiniProyectoRedSocial.Core.Application.ViewModels.Comentarios;

namespace MiniProyectoRedSocial.Controllers
{
    public class AmigoController : Controller
    {
        private readonly IAmigoService _amigoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPublicacionService _publicacionService;
        private readonly IComentarioService _comentarioService;
        private readonly ValidateUserSession _validateUserSession;

        public AmigoController(IAmigoService amigoService, IUsuarioService usuarioService, ValidateUserSession validateUserSession, IPublicacionService publicacionService, IComentarioService comentarioService)
        {
            _amigoService = amigoService;
            _usuarioService = usuarioService;
            _publicacionService = publicacionService;
            _comentarioService = comentarioService;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            var amigosViewModel = await _amigoService.GetAllViewModel();
            var amigoIds = amigosViewModel.Select(a => a.AmigoId).ToList();
            var publicacionesViewModel = await _publicacionService.GetPublicacionesDeAmigos(amigoIds);

            ViewBag.Publicaciones = publicacionesViewModel;
            publicacionesViewModel.Reverse();
            return View(amigosViewModel);
        }

        [HttpGet]
        public IActionResult AgregarAmigo()
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            return View("AgregarAmigo");
        }

        [HttpPost]
        public async Task<IActionResult> AgregarAmigo(string nombreUsuario)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            var usuario = await _usuarioService.GetByNombreUsuario(nombreUsuario);

            if (usuario == null)
            {
                ModelState.AddModelError("nombreUsuario", "El nombre de usuario no existe.");
                ViewBag.Message = "No existe ese usuario.";
                return View("AgregarAmigo");
            }

            int usuarioId = ObtenerUsuarioActualId();

            if (usuario.Id == usuarioId)
            {
                ViewBag.Message = "No te puedes agregar como amigo.";
                return View("AgregarAmigo");
            }

            var amigosDelUsuario = await _amigoService.GetAmigosByUsuarioId(usuarioId);
            if (amigosDelUsuario.Any(a => a.AmigoId == usuario.Id))
            {
                ViewBag.Message = $"Ya tienes a {usuario.NombreUsuario} como amigo.";
                return View("AgregarAmigo");
            }

            await _amigoService.AddAmigo(usuarioId, usuario.Id);
            ViewBag.Message = $"Amigo {usuario.NombreUsuario} agregado correctamente.";

            return View("AgregarAmigo");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            var amigoVm = await _amigoService.GetByIdSaveViewModel(id);
            return View(amigoVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            var amigoVm = await _amigoService.GetByIdSaveViewModel(id);
            await _amigoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private int ObtenerUsuarioActualId()
        {
            var usuarioViewModel = HttpContext.Session.Get<UsuarioViewModel>("usuario");
            return usuarioViewModel?.Id ?? 0;
        }

        [HttpPost]
        public async Task<IActionResult> AddComentario(SaveComentarioViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _comentarioService.AddComentarioToPublicacion(vm);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar comentario: " + ex.Message);
                }
            }

            var amigosViewModel = await _amigoService.GetAllViewModel();
            var amigoIds = amigosViewModel.Select(a => a.AmigoId).ToList();
            var publicacionesViewModel = await _publicacionService.GetPublicacionesDeAmigos(amigoIds);
            ViewBag.Publicaciones = publicacionesViewModel;

            return View("Index", amigosViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubComentario(SaveComentarioViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _comentarioService.AddSubComentario(vm);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar subcomentario: " + ex.Message);
                }
            }

            return RedirectToAction("Index");
        }
    }
}