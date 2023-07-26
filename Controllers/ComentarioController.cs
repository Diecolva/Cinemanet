using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinemanet.Models.Domain;
using System.Security.Claims;

namespace Cinemanet.Controllers
{
    public class ComentariosController : Controller
    {
        private readonly DatabaseContext _context;

        public ComentariosController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Comentarios
        public async Task<IActionResult> Index()
        {
            var comentarios = await _context.Comentarios.ToListAsync();
            return View(comentarios);
        }

        // GET: Comentarios/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // GET: Comentarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comentarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(long peliculaId, string contenido)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Obtener el ID del usuario autenticado
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var comentario = new Comentario
            {
                PeliculaId = peliculaId,
                UsuarioId = usuarioId,
                Contenido = contenido,
                Fecha = DateTime.Now
            };

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "User", new { id = peliculaId });
        }

        // GET: Comentarios/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // POST: Comentarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Comentario comentario)
        {
            if (id != comentario.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(comentario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(comentario);
        }

        // GET: Comentarios/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // POST: Comentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var comentario = await _context.Comentarios
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comentario == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (comentario.UsuarioId != userId)
            {
                return Forbid();
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            ViewData["Mensaje"] = "Comentario eliminado correctamente.";

            var pelicula = await _context.Peliculas.FindAsync(comentario.PeliculaId);

            if (pelicula != null)
            {
                await _context.Entry(pelicula).Collection(c => c.Comentarios).LoadAsync();
            }

            return View("~/Views/User/Details.cshtml", pelicula);
        }
    }
}