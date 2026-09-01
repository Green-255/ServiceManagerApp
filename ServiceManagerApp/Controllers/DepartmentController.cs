using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.ViewModels.Departments;

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
            var viewModel = new DepartmentCreateEditViewModel
            {
                JobRoles = await GetJobRoleCheckboxesAsync(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentCreateEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.JobRoles = await GetJobRoleCheckboxesAsync(viewModel.JobRoleIds);
                return View(viewModel);
            }

            var departmentToAdd = new Department
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
            };

            await _context.AddAsync(departmentToAdd);
            await _context.SaveChangesAsync();


            var allJobRoles = await _context.JobRoles
                .Where(jr => viewModel.JobRoleIds.Contains(jr.Id))
                .ToListAsync();

            foreach(var role in allJobRoles)
            {
               role.DepartmentId = departmentToAdd.Id;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            //var department = await _context.Departments.FindAsync(id);
            var department = await _context.Departments
                .Include(d => d.JobRoles)
                .FirstOrDefaultAsync(d => d.Id == id);

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

            var department = await _context.Departments
                .Include(d => d.JobRoles)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }


            var jobRoleIds = _context.JobRoles.Select(jr => jr.Id).ToList(); // why no async await?
            var selectedIds = department.JobRoles.Select(jr => jr.Id).ToList();

            var viewModel = new DepartmentCreateEditViewModel
            {
                Name = department.Name,
                Description = department.Description,
                JobRoleIds = jobRoleIds,
                JobRoles = await GetJobRoleCheckboxesAsync(selectedIds),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentCreateEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var departmentToEdit = await _context.Departments.FindAsync(viewModel.Id);

            if (departmentToEdit == null)
            {
                return NotFound();
            }

            departmentToEdit.Name = viewModel.Name;
            departmentToEdit.Description = viewModel.Description;


            var allJobRoles = await _context.JobRoles.ToListAsync();


            // maybe it is possible to use XOR here to optimize.
            // XOR operation would be used on Ids arrays (lists) of before and after
            foreach (JobRole role in allJobRoles)
            {
                if (viewModel.JobRoleIds.Contains(role.Id))
                {
                    role.DepartmentId = departmentToEdit.Id;
                }
                // nelabai suprantu kam sita
                else if (role.DepartmentId == departmentToEdit.Id)
                {
                    role.DepartmentId = null;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = departmentToEdit.Id });
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

        private async Task<List<JobRoleCheckboxViewModel>> GetJobRoleCheckboxesAsync(
            List<int>? selectedJobRoleIds = null)
        {
            selectedJobRoleIds ??= [];

            return await _context.JobRoles
                .OrderBy(jr => jr.Name)
                .Select(jr => new JobRoleCheckboxViewModel
                {
                    Id = jr.Id,
                    Name = jr.Name,
                    IsSelected = selectedJobRoleIds.Contains(jr.Id)
                })
                .ToListAsync();
        }

    }
}
