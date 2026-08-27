using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;

namespace ServiceManagerApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments.ToListAsync();
            return View(departments);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }

            await _context.AddAsync(departmentVM);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var departmentToEdit = await _context.Departments.FindAsync(department.Id);

            if (departmentToEdit == null)
            {
                return NotFound();
            }

            departmentToEdit.Name = department.Name;
            departmentToEdit.Description = department.Description;
            departmentToEdit.JobRoles = department.JobRoles;

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

            var deleted = await _context.Departments.Where(d => d.Id == id).ExecuteDeleteAsync();
            if (deleted == 0)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
