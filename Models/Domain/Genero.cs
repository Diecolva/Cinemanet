namespace Cinemanet.Models.Domain
{
    public partial class Genero
    {
        public Genero()
        {
            Peliculas = new HashSet<Pelicula>();
        }
        public long Id { get; set; }
        public string? Nombre { get; set; }

        public ICollection<Pelicula> Peliculas { get; set; }
    }
}