namespace Cinemanet.Models.Domain
{
        public class Comentario
        {
            public long Id { get; set; }

            public long PeliculaId { get; set; }
            public virtual Pelicula? Pelicula { get; set; }
            public string? UsuarioId { get; set; }
            public virtual ApplicationUser Usuario { get; set; } 
            public string? Contenido { get; set; }
            public DateTime Fecha { get; set; }    


        }
}