using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniProyectoRedSocial.Core.Application.Interfaces.Services;
using MiniProyectoRedSocial.Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IAmigoService, AmigoService>();
            services.AddTransient<IComentarioService, ComentarioService>();
            services.AddTransient<IPublicacionService, PublicacionService>();
            #endregion
        }
    }
}
