using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinemanet.Models;
using Cinemanet.Models.Domain;
using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Cinemanet.Controllers
{
    public class PeliculaController : Controller
    {
        private readonly DatabaseContext _context;

        public PeliculaController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Pelicula
        public IActionResult Index()
        {
            var peliculas = _context.Peliculas.Include(p => p.Generos).ToList();
            return View(peliculas);
        }

        // GET: Pelicula/Details/5
        public IActionResult Details(int id)
        {
            var pelicula = _context.Peliculas.Include(p => p.Generos).FirstOrDefault(p => p.Id == id);

            if (pelicula == null)
                return NotFound();

            return View(pelicula);
        }

        // GET: Pelicula/Create
        public IActionResult Create()
        {
            var generos = _context.Generos.ToList();
            ViewBag.Generos = generos;

            return View();
        }

        // POST: Pelicula/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pelicula pelicula, long[] generos, IFormFile caratula)
        {
            if (ModelState.IsValid)
            {
                if (caratula != null && caratula.Length > 0)
                {
                    var extension = Path.GetExtension(caratula.FileName);
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        caratula.CopyTo(fileStream);
                    }

                    pelicula.Caratula = "/uploads/" + fileName;
                }

                if (generos != null)
                {
                    var selectedGeneros = _context.Generos.Where(g => generos.Contains(g.Id)).ToList();
                    pelicula.Generos = selectedGeneros;
                }

                _context.Peliculas.Add(pelicula);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var allGeneros = _context.Generos.ToList();
            ViewBag.Generos = allGeneros;

            return View(pelicula);
        }

        // GET: Pelicula/Edit/5
        public IActionResult Edit(int id)
        {
            var pelicula = _context.Peliculas.Include(p => p.Generos).FirstOrDefault(p => p.Id == id);

            if (pelicula == null)
                return NotFound();

            var generos = _context.Generos.ToList();
            ViewBag.Generos = generos;

            var generosAsociados = pelicula.Generos.Select(g => g.Id).ToList();
            ViewBag.GenerosAsociados = generosAsociados;

            return View(pelicula);
        }

        // POST: Pelicula/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pelicula pelicula, long[] generos, IFormFile caratula)
        {
            if (id != pelicula.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var peliculaExistente = _context.Peliculas.Include(p => p.Generos).FirstOrDefault(p => p.Id == id);

                if (peliculaExistente == null)
                    return NotFound();

                if (caratula != null && caratula.Length > 0)
                {
                    var extension = Path.GetExtension(caratula.FileName);
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        caratula.CopyTo(fileStream);
                    }

                    peliculaExistente.Caratula = "/uploads/" + fileName;
                }
                
                if (generos != null)
                {
                    peliculaExistente.Generos.Clear();
                    var selectedGeneros = _context.Generos.Where(g => generos.Contains(g.Id)).ToList();
                    foreach (var genero in selectedGeneros)
                    {
                        peliculaExistente.Generos.Add(genero);
                    }
                }

                peliculaExistente.Nombre = pelicula.Nombre;
                peliculaExistente.Sinopsis = pelicula.Sinopsis;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var generosList = _context.Generos.ToList();
            ViewBag.Generos = generosList;

            return View(pelicula);
        }

        // GET: Pelicula/Delete/5
        public IActionResult Delete(int id)
        {
            var pelicula = _context.Peliculas.Include(p => p.Generos).FirstOrDefault(p => p.Id == id);
            if (pelicula == null)
                return NotFound();

            return View(pelicula);
        }

        // POST: Pelicula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var pelicula = _context.Peliculas.Include(p => p.Generos).FirstOrDefault(p => p.Id == id);
            if (pelicula == null)
                return NotFound();

            _context.Peliculas.Remove(pelicula);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}