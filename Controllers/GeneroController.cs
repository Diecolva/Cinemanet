using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinemanet.Models.Domain;

namespace Cinemanet.Controllers
{
    public class GeneroController : Controller
    {
        private readonly DatabaseContext _context;

        public GeneroController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Genero
        public IActionResult Index()
        {
            var generos = _context.Generos.Include(g => g.Peliculas).ToList();
            return View(generos);
        }

        // GET: Genero/Details/5
        public IActionResult Details(int id)
        {
            var genero = _context.Generos.Include(g => g.Peliculas).FirstOrDefault(g => g.Id == id);
            if (genero == null)
                return NotFound();

            return View(genero);
        }

        // GET: Genero/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genero/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genero genero)
        {
            if (ModelState.IsValid)
            {
                _context.Generos.Add(genero);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(genero);
        }

        // GET: Genero/Edit/5
        public IActionResult Edit(int id)
        {
            var genero = _context.Generos.FirstOrDefault(g => g.Id == id);
            if (genero == null)
                return NotFound();

            return View(genero);
        }

        // POST: Genero/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Genero genero)
        {
            if (id != genero.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Generos.Update(genero);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(genero);
        }

        // GET: Genero/Delete/5
        public IActionResult Delete(int id)
        {
            var genero = _context.Generos.FirstOrDefault(g => g.Id == id);
            if (genero == null)
                return NotFound();

            return View(genero);
        }

        // POST: Genero/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var genero = _context.Generos.FirstOrDefault(g => g.Id == id);
            if (genero == null)
                return NotFound();

            _context.Generos.Remove(genero);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}