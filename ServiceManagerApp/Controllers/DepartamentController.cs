using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;

namespace ServiceManagerApp.Controllers
{
    public class DepartamentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartamentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departaments = await _context.Departaments.ToListAsync();
            return View(departaments);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Departament departamentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departamentVM);
            }

            await _context.AddAsync(departamentVM);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var departament = await _context.Departaments.FindAsync(id);

            if (departament == null)
            {
                return NotFound();
            }

            return View(departament);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var departament = await _context.Departaments.FindAsync(id);

            if (departament == null)
            {
                return NotFound();
            }

            return View(departament);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Departament departament)
        {
            if (!ModelState.IsValid)
            {
                return View(departament);
            }

            var departamentToEdit = await _context.Departaments.FindAsync(departament.Id);

            if (departamentToEdit == null)
            {
                return NotFound();
            }

            departamentToEdit.Name = departament.Name;
            departamentToEdit.Description = departament.Description;
            departamentToEdit.JobRoles = departament.JobRoles;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            var deleted = await _context.Departaments.Where(d => d.Id == id).ExecuteDeleteAsync();
            if (deleted == 0)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
