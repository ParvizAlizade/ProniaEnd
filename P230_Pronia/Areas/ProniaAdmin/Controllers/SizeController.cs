using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P230_Pronia.DAL;
using P230_Pronia.Entities;
using P230_Pronia.Utilities.Roles;

namespace P230_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class SizeController : Controller
    {
        private readonly ProniaDbContext _context;

        public SizeController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Size> Sizes = _context.Sizes.AsEnumerable();
            return View(Sizes);
        }
        public IActionResult Create(int id)
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Size newSize)
        {
            if (newSize == null) return NotFound();
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }

                return View();
            }
            bool isDuplicated = _context.Sizes.Any(c => c.Name == newSize.Name);
            if (isDuplicated)
            {
                ModelState.AddModelError("", "You cannot duplicate value");
                return View();
            }
            _context.Sizes.Add(newSize);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0) return NotFound();
            Size Size = _context.Sizes.FirstOrDefault(c => c.Id == id);
            if (Size is null) return NotFound();
            return View(Size);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public IActionResult Edit(int id, Size edited)
        {
            if (id <= 0) return NotFound();
            Size Size = _context.Sizes.FirstOrDefault(e => e.Id == id);
            if (Size is null) return NotFound();
            bool duplicate = _context.Sizes.Any(c => c.Name == edited.Name && edited.Name != Size.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate category name");
                return View(Size);
            }
            Size.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0) return NotFound();
            Size Size = _context.Sizes.FirstOrDefault(c => c.Id == id);
            if (Size is null) return NotFound();
            return View(Size);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int id, Size deleted)
        {
            if (deleted.Id <= 0) return BadRequest();
            Size Size = _context.Sizes.FirstOrDefault(c => c.Id == id);
            if (Size is null) return NotFound();
            _context.Sizes.Remove(Size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();
            Size Size = _context.Sizes.FirstOrDefault(c => c.Id == id);
            if (Size is null) return NotFound();
            return View(Size);
        }
    }
}
