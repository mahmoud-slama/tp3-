using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP3.Models.Cinema;

namespace TP3.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaDbContext _context;

        public MoviesController(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MoviesAndtheirProds()
        {
            var cinemaDbContext = _context.Movies.Include(m => m.Producer);

            return View(await cinemaDbContext.ToListAsync());
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var cinemaDbContext = _context.Movies.Include(m => m.Producer);
            return View(await cinemaDbContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            try
            {
             
                Console.WriteLine($" movie >> {movie}");
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
                return View(movie);
            }
            catch
            {
                return NotFound();       
            
            }
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

          
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                   return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
             
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public ActionResult MoviesAndTheirProds_UsingModel()
        {
            var requet = from m in _context.Movies
                         join p in _context.Producers
                         on m.ProducerId equals p.Id
                         select new ProdMovie
                         {
                             mGenre = m.Genre,
                             mTitle = m.Title,
                             
                         };
            Console.WriteLine($" your requet hit ; {requet}");
            return View(requet);
        }

      
        public ActionResult SeachByTitle(string search="")
        {
            var seachInDataBase = from m in _context.Movies
                                  where m.Title.Contains(search)
                                  select m;
            return View(seachInDataBase);
        }

        public ActionResult SearchByGenre(string genre = "")
        {
            var movieByGenre = from m in _context.Movies.Include(m => m.Producer)
                               where m.Genre.Contains(genre)
                               select m;

            return View(movieByGenre);
        }
        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
