namespace Cinemanet.Models.Domain
{
    public partial class Pelicula
    {
        public Pelicula()
        {
            Generos = new HashSet<Genero>();
            Comentarios = new HashSet<Comentario>();
        }
        public long Id { get; set; }
        public string? Nombre { get; set; }
        public string? Sinopsis { get; set; }
        public string? Caratula { get; set; }
        public ICollection<Genero> Generos { get; set; }
        public ICollection<Comentario> Comentarios { get; set; }

    }
}