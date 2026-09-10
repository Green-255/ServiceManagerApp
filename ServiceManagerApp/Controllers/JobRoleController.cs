using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.ViewModels.JobRoles;

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
            var roles = await _context.JobRoles
                .Include(jr => jr.Department)
                .ToListAsync();
            return View(roles);
        }

        public async Task<IActionResult> Create()
        {
            var model = new JobRoleCreateEditViewModel
            {
                Departments = await PopulateDepartmentDropDownAsync()
            };

            return View(model);
        }

        public async Task<List<SelectListItem>> PopulateDepartmentDropDownAsync()
        {
            return await _context.Departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            })
            .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobRoleCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await PopulateDepartmentDropDownAsync();
                return View(model);
            }

            JobRole jobRole = new JobRole
            {
                Name = model.Name,
                Description = model.Description,
                DepartmentId = model.DepartmentId,
            };

            await _context.AddAsync(jobRole);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var role = await _context.JobRoles
                .Include(jr => jr.Department)
                .FirstOrDefaultAsync(jr => jr.Id == id);

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

            var roleVM = new JobRoleCreateEditViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                DepartmentId = role.DepartmentId,
                Departments = await PopulateDepartmentDropDownAsync(),
            };

            return View(roleVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(JobRoleCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await PopulateDepartmentDropDownAsync();
                return View(model);
            }

            var roleToEdit = await _context.JobRoles.FindAsync(model.Id);

            if (roleToEdit == null)
            {
                return NotFound();
            }

            roleToEdit.Name = model.Name;
            roleToEdit.Description = model.Description;
            roleToEdit.DepartmentId = model.DepartmentId;
            //roleToEdit.Workers = roleVM.Workers;

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
