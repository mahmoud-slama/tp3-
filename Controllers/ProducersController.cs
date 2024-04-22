using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP3.Models.Cinema;

namespace TP3.Controllers
{
     public class ProducersController : Controller
    {
        CinemaDbContext _context;
        public ProducersController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: ProducersController
        public ActionResult Index()
        {
            return View(_context.Producers.ToList());
        }

        // GET: ProducersController/Details/5
        public ActionResult Details(int id)
        {

            try
            {
                Producer prod = _context.Producers.Find(id);
                if (prod == null)
                {
                    // If the producer is not found, return a NotFound result
                    return NotFound();
                }
                return View(prod);
            }
            catch
            {
                return NotFound();

            }
         }

        // GET: ProducersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProducersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Producer newProducer = new Producer
                {
                    Name = collection["Name"],
                    Nationality = collection["Nationality"],
                    Email= collection["Email"],
                };
                _context.Producers.Add(newProducer);
                _context.SaveChanges();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Producer prod = _context.Producers.Find(id);
                if (prod == null)
                {
                    // If the producer is not found, return a NotFound result
                    return NotFound();
                }
                return View(prod);
            }
            catch
            {
                return NotFound();

            }

        }

        // POST: ProducersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Producer producer)
        {
            if(id != producer.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Producers.Update(producer);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(producer);
                }

            }
            return View(producer);
        }

        // GET: ProducersController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Producer prodDelet = _context.Producers.Find(id);
                if (prodDelet == null)
                {
                    // If the producer is not found, return a NotFound result
                    return NotFound();
                }
                return View(prodDelet);

            }catch
            {
                return BadRequest();
            }
        }



          public ActionResult MyMovies(int id)
        {
            var requet = from m in _context.Movies
                         join p in _context.Producers
                         on m.ProducerId equals p.Id
                         where p.Id == id
                         select new ProdMovie
                         {
                             mGenre = m.Genre,
                             mTitle = m.Title,
                             pName = p.Name,
                             pNat = p.Nationality,
                         };

            return View(requet);

        }




        // POST: ProducersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                Producer prodDelet = _context.Producers.Find(id);
                if (prodDelet == null)
                {
                    // If the producer is not found, return a NotFound result
                    return NotFound();
                }
                _context.Producers.Remove(prodDelet);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
