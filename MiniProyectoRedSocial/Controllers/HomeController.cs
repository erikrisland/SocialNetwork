using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProyectoRedSocial.Core.Application.Interfaces.Services;
using MiniProyectoRedSocial.Core.Application.ViewModels.Publicaciones;
using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
using MiniProyectoRedSocial.Core.Application.Helpers;
using MiniProyectoRedSocial.Core.Application.ViewModels.Comentarios;
using MiniProyectoRedSocial.Middlewares;

namespace MiniProyectoRedSocial.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPublicacionService _publicacionService;
        private readonly IComentarioService _comentarioService;
        private readonly ValidateUserSession _validateUserSession;

        public HomeController(IPublicacionService publicacionService, IComentarioService comentarioService, ValidateUserSession validateUserSession)
        {
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

           var usuarioId = ObtenerUsuarioActualId();
           var publicaciones = await _publicacionService.GetPublicacionesByUsuarioId(usuarioId);

            foreach (var publicacion in publicaciones)
            {
                publicacion.Comentarios = await _comentarioService.GetComentariosByPublicacionId(publicacion.Id);
            }

            publicaciones.Reverse();
            return View(publicaciones);
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
                    if (vm.ComentarioPadreId.HasValue)
                    {
                        await _comentarioService.AddSubComentario(vm);
                    }
                    else
                    {
                        await _comentarioService.AddComentarioToPublicacion(vm);
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar comentario: " + ex.Message);
                }
            }

            return View(vm);
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
                    var subComentario = await _comentarioService.AddSubComentario(vm);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar subcomentario: " + ex.Message);
                }
            }

            var publicaciones = await _publicacionService.GetAllViewModel();
            foreach (var publicacion in publicaciones)
            {
                publicacion.Comentarios = await _comentarioService.GetComentariosByPublicacionId(publicacion.Id);
            }
            publicaciones.Reverse();
            return View("Index", publicaciones);
        }

        public IActionResult Create()
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            return View("SavePublicacion", new SavePublicacionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePublicacionViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid)
            {
                return View("SavePublicacion", vm);
            }

            if (vm.File != null && vm.File.Length > 0)
            {
                vm.Imagen = UploadFile(vm.File, vm.UsuarioId);
            }

            vm.FechaHora = DateTime.Now;
            await _publicacionService.Add(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            var publicacionVm = await _publicacionService.GetByIdSaveViewModel(id);
            return View("SavePublicacion", publicacionVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePublicacionViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid)
            {
                return View("SavePublicacion", vm);
            }

            var existingPublicacion = await _publicacionService.GetByIdSaveViewModel(vm.Id);

            if (vm.File != null && vm.File.Length > 0)
            {
                vm.Imagen = UploadFile(vm.File, vm.UsuarioId, true, existingPublicacion.Imagen);
            }
            else
            {
                vm.Imagen = existingPublicacion.Imagen;
            }

            await _publicacionService.Update(vm, vm.Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            var publicacionVm = await _publicacionService.GetByIdSaveViewModel(id);
            return View(publicacionVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                TempData["ErrorMensaje"] = "No tienes permiso para acceder a estas secciones, tienes que iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            await _publicacionService.Delete(id);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        private string UploadFile(IFormFile file, int userId, bool isEditMode = false, string oldImagePath = "")
        {
            if (file == null || file.Length == 0)
            {
                return isEditMode ? oldImagePath : null;
            }

            string basePath = $"/Images/Publicaciones/{userId}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            string extension = Path.GetExtension(file.FileName);
            string fileName = $"{guid}{extension}";

            string filePath = Path.Combine(path, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode && !string.IsNullOrEmpty(oldImagePath))
            {
                string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{oldImagePath}");
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            return $"{basePath}/{fileName}";
        }

        private int ObtenerUsuarioActualId()
        {
            var usuarioViewModel = HttpContext.Session.Get<UsuarioViewModel>("usuario");
            return usuarioViewModel?.Id ?? 0;
        }
    }
}