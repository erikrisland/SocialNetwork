using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProyectoRedSocial.Core.Application.Interfaces.Services;
using MiniProyectoRedSocial.Core.Application.ViewModels.Usuarios;
using MiniProyectoRedSocial.Core.Application.Helpers;
using MiniProyectoRedSocial.Core.Application.Dtos.Email;

namespace MiniProyectoRedSocial.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEmailService _emailService;

        public LoginController(IUsuarioService usuarioService, IEmailService emailService)
        {
            _usuarioService = usuarioService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MensajeActivacion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            UsuarioViewModel usuarioVm = await _usuarioService.Login(loginVm);

            if (usuarioVm != null)
            {
                if (usuarioVm.Activo)
                {
                    HttpContext.Session.Set<UsuarioViewModel>("usuario", usuarioVm);
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }
                else
                {
                    ModelState.AddModelError("userValidation", "Cuenta no activada. Verifique su correo electrónico para activar la cuenta.");
                }
            }
            else
            {
                ModelState.AddModelError("userValidation", "Datos de acceso incorrectos.");
            }

            return View(loginVm);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("usuario");
            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }

        public IActionResult Registro()
        {
            return View(new SaveUsuarioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Registro(SaveUsuarioViewModel usuarioVm)
        {
            if (!ModelState.IsValid)
            {
                return View(usuarioVm);
            }

            var existingUser = await _usuarioService.GetByNombreUsuario(usuarioVm.NombreUsuario);
            if (existingUser != null)
            {
                ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya está en uso.");
                return View(usuarioVm);
            }
            else
            {
                usuarioVm.FotoPerfil = UploadFile(usuarioVm.File, usuarioVm.Id);
                await _usuarioService.Add(usuarioVm);
            }

            return RedirectToRoute(new { controller = "Login", action = "MensajeActivacion" });
        }

        public async Task<IActionResult> ActivateAccount(string token)
        {
            var user = await _usuarioService.GetByActivationToken(token);
            if (user == null)
            {
                return NotFound();
            }

            user.Activo = true;

            SaveUsuarioViewModel saveUserVm = new SaveUsuarioViewModel
            {
                Id = user.Id,
                Nombre = user.Nombre,  
                Apellido = user.Apellido,    
                NombreUsuario = user.NombreUsuario,
                Correo = user.Correo,
                Contraseña = user.Contraseña,
                Telefono = user.Telefono,      
                FotoPerfil = user.FotoPerfil,
                Activo = true,
                ActivationToken = user.ActivationToken
            };

            await _usuarioService.Update(saveUserVm, user.Id);

            return RedirectToAction("Index", "Login");
        }

        private string UploadFile(IFormFile file, int id)
        {
            if (file == null)
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
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"{basePath}/{fileName}";
        }

        public IActionResult RestablecerContra()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RestablecerContra(string nombreUsuario)
        {
            var usuario = await _usuarioService.GetByNombreUsuario(nombreUsuario);
            if (usuario == null)
            {
                ModelState.AddModelError("userValidation", "El nombre de usuario no existe.");
                return View();
            }

            string nuevaContraseña = GenerarContraseñaAleatoria();
            string nuevaContraseñaEncriptada = PasswordInscryption.ComputeSha256Hash(nuevaContraseña);

            var saveUsuarioViewModel = new SaveUsuarioViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Telefono,
                Correo = usuario.Correo,
                FotoPerfil = usuario.FotoPerfil,
                NombreUsuario = usuario.NombreUsuario,
                Contraseña = nuevaContraseñaEncriptada,
                Activo = usuario.Activo,
                ActivationToken = usuario.ActivationToken
            };

            await _usuarioService.Update(saveUsuarioViewModel, usuario.Id);

            await _emailService.SendAsync(new EmailRequest
            {
                To = usuario.Correo,
                Subject = "Restablecimiento de Contraseña",
                Body = $"<h4>Hola {usuario.Nombre},</h4><p>Tu nueva contraseña es: <strong>{nuevaContraseña}</strong></p>"
            });

            ViewBag.Message = "La nueva contraseña ha sido enviada a su correo electrónico.";
            return View();
        }

        private string GenerarContraseñaAleatoria()
        {
            var caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var contraseña = new char[10];
            var random = new Random();

            for (int i = 0; i < contraseña.Length; i++)
            {
                contraseña[i] = caracteres[random.Next(caracteres.Length)];
            }

            return new string(contraseña);
        }
    }
}