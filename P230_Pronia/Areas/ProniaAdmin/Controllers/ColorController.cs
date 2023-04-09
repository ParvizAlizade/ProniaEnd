using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    [Authorize(Roles ="Admin,Moderator")]
    public class ColorController : Controller
    {
        private readonly ProniaDbContext _context;

        public ColorController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Color> colors = _context.Colors.AsEnumerable();
            return View(colors);
        }
        public IActionResult Create(int id)
        {
          return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Color newcolor) 
        {
            if (newcolor == null) return NotFound();
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }

                return View();
            }
            bool isDuplicated = _context.Colors.Any(c => c.Name == newcolor.Name);
            if (isDuplicated)
            {
                ModelState.AddModelError("", "You cannot duplicate value");
                return View();
            }
            _context.Colors.Add(newcolor);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0) return NotFound();
            Color color = _context.Colors.FirstOrDefault(c => c.Id == id);
            if (color is null) return NotFound();
            return View(color);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public IActionResult Edit(int id,Color edited)
        {
            if (id <= 0) return NotFound(); 
            Color color = _context.Colors.FirstOrDefault(e => e.Id == id);
            if (color is null) return NotFound();
            bool duplicate = _context.Colors.Any(c => c.Name == edited.Name && edited.Name != color.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate category name");
                return View(color);
            }
            color.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0) return NotFound();
            Color color = _context.Colors.FirstOrDefault(c => c.Id == id);
            if (color is  null) return NotFound();
            return View(color);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int id,Color deleted)
        {
            if (deleted.Id <= 0) return BadRequest();
            Color color = _context.Colors.FirstOrDefault(c=>c.Id == id);
            if (color is null) return NotFound();
            _context.Colors.Remove(color);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();
            Color color = _context.Colors.FirstOrDefault(c => c.Id == id);
            if (color is null) return NotFound();
            return View(color);
        }


    }
}
