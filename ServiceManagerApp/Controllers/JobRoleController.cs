using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using ServiceManagerApp.Models.ViewModels.JobRoles;
using ServiceManagerApp.Services;

namespace ServiceManagerApp.Controllers
{
    public class JobRoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly LookupService _lookupService;

        public JobRoleController (ApplicationDbContext context, LookupService lookupService)
        {
            _context = context;
            _lookupService = lookupService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _context.JobRoles
                .Select(jr => new JobRoleIndexViewModel
                {
                    Id = jr.Id,
                    Name = jr.Name,
                    Description = jr.Description,
                    DepartmentId = jr.DepartmentId,
                    DepartmentName = jr.Department != null
                        ? jr.Department.Name
                        : null,
                    NumberOfWorkers = jr.Workers.Count()
                })
                .ToListAsync();

            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new JobRoleCreateEditViewModel
            {
                Departments = await _lookupService.PopulateDepartmentDropDownAsync()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobRoleCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await _lookupService.PopulateDepartmentDropDownAsync();
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

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var role = await _context.JobRoles
                .Where(jr =>  jr.Id == id)
                //.Include(jr => jr.Department) // can remove .Include() calls, since you're projecting 
                //.Include(jr => jr.Workers)    // the required properties directly using .Select().
                .Select(jr => new JobRoleDetailsViewModel
                {
                    Id = jr.Id,
                    Name = jr.Name,
                    Description = jr.Description,
                    DepartmentId = jr.DepartmentId != null
                        ? jr.DepartmentId
                        : null,
                    DepartmentName = jr.Department != null
                        ? jr.Department.Name
                        : null,
                    Workers = jr.Workers.Select(w => new WorkerForJobRoleViewModel
                    {
                        Id = w.Id,
                        ReferenceNumber = w.ReferenceNumber,
                        Name = w.Name,
                        AvailabilityStatus = w.AvailabilityStatus.ToString(),
                        SkillLevel = w.SkillLevel.HasValue
                            ? w.SkillLevel.Value.ToString()
                            : string.Empty
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpGet]
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
                Departments = await _lookupService.PopulateDepartmentDropDownAsync(),
            };

            return View(roleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobRoleCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await _lookupService.PopulateDepartmentDropDownAsync();
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

            return RedirectToAction(nameof(Details), new {id = roleToEdit.Id});
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
