using Cinemanet.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinemanet.Controllers

{
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public UserController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Cinemanet()
        {
            var peliculas = _dbContext.Peliculas.Include(p => p.Generos).ToList();
            return View(peliculas);
        }

        public async Task<IActionResult> Details(long id)
        {
            var pelicula = await _dbContext.Peliculas
                .Include(p => p.Generos)
                .Include(p => p.Comentarios)
                .ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }
    }
}