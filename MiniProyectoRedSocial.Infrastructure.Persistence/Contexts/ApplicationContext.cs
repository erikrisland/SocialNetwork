using Microsoft.EntityFrameworkCore;
using MiniProyectoRedSocial.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Infrastructure.Persistence.Contexts
{ 
    public class ApplicationContext : DbContext
    {
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Publicacion> Publicaciones { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }
    public DbSet<Amigo> Amigos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent API

            #region Tablas
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Publicacion>().ToTable("Publicaciones");
            modelBuilder.Entity<Comentario>().ToTable("Comentarios");
            modelBuilder.Entity<Amigo>().ToTable("Amigos");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Publicacion>().HasKey(p => p.Id);
            modelBuilder.Entity<Comentario>().HasKey(c => c.Id);
            modelBuilder.Entity<Amigo>().HasKey(a => a.Id);
            #endregion

            #region Relationships
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Publicaciones)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Amigos)
                .WithOne(a => a.Usuario)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Amigo>()
                .HasOne(a => a.AmigoUsuario)
                .WithMany()
                .HasForeignKey(a => a.AmigoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Publicacion>()
                .HasMany(p => p.Comentarios)
                .WithOne(c => c.Publicacion)
                .HasForeignKey(c => c.PublicacionId);

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comentario>()
                .HasMany(c => c.SubComentarios)
                .WithOne()
                .HasForeignKey(c => c.ComentarioPadreId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Property Configurations

            #region Publicacion
            modelBuilder.Entity<Publicacion>()
                .Property(p => p.Imagen)
                .IsRequired(false);

            modelBuilder.Entity<Publicacion>()
                .Property(p => p.VideoUrl)
                .IsRequired(false);
            #endregion

            #region Comentario
            modelBuilder.Entity<Comentario>()
                .Property(c => c.ComentarioPadreId)
                .IsRequired(false);
            #endregion

            #endregion
        }
    }

}
