using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cinemanet.Models.Domain;

namespace Cinemanet.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pelicula>(entity =>
            {
                entity.ToTable("Peliculas");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Sinopsis)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.Caratula)
                    .IsRequired();

                entity.HasMany(e => e.Generos)
                    .WithMany(g => g.Peliculas)
                    .UsingEntity<Dictionary<string, object>>(
                        "PeliculaGenero",
                        j => j
                            .HasOne<Genero>()
                            .WithMany()
                            .HasForeignKey("GeneroId")
                            .HasConstraintName("FK_PeliculaGenero_Generos_GeneroId")
                            .OnDelete(DeleteBehavior.Cascade),
                        j => j
                            .HasOne<Pelicula>()
                            .WithMany()
                            .HasForeignKey("PeliculaId")
                            .HasConstraintName("FK_PeliculaGenero_Peliculas_PeliculaId")
                            .OnDelete(DeleteBehavior.Cascade)
                    );

                entity.HasMany(e => e.Comentarios)
                    .WithOne(c => c.Pelicula)
                    .HasForeignKey(c => c.PeliculaId);
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.ToTable("Generos");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsRequired();
            });

            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.ToTable("Comentarios");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Contenido)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.Fecha)
                    .IsRequired();

                entity.HasOne(e => e.Pelicula)
                    .WithMany(c => c.Comentarios)
                    .HasForeignKey(e => e.PeliculaId);

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.UsuarioId)
                    .IsRequired();
            });
        }
    }
}