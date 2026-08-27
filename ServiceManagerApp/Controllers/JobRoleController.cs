using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;

namespace ServiceManagerApp.Controllers
{
    public class JobRoleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobRoleController (ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var roles = await _context.JobRoles.ToListAsync();
            return View(roles);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobRole jobRoleVM)
        {
            if (!ModelState.IsValid)
            {
                return View(jobRoleVM);
            }

            await _context.AddAsync(jobRoleVM);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var role = await _context.JobRoles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var role = await _context.JobRoles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(JobRole role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var roleToEdit = await _context.JobRoles.FindAsync(role.Id);

            if (roleToEdit == null)
            {
                return NotFound();
            }

            roleToEdit.Name = role.Name;
            roleToEdit.Description = role.Description;
            roleToEdit.Departament = role.Departament;
            roleToEdit.Workers = role.Workers;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var deleted = await _context.JobRoles.Where(jr => jr.Id == id).ExecuteDeleteAsync();
            if (deleted == 0)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
