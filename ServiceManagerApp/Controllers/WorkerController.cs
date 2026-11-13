using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.ViewModels.Workers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ServiceManagerApp.Authorization;
using ServiceManagerApp.Services.Interfaces;

namespace ServiceManagerApp.Controllers;

[Authorize(Roles = AppRoles.Worker + "," + AppRoles.Administrator)]
public class WorkerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly LookupService _lookupService;
    public WorkerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
        LookupService lookupService)
    {
        _context = context;
        _userManager = userManager;
        _lookupService = lookupService;
    }

    public async Task<IActionResult> Index()
    {
        var workersList = await _context.Workers
            .Include(w => w.JobRole)
            .Select(w => new WorkerIndexViewModel
        {
            Id              = w.Id,
            ReferenceNumber = w.ReferenceNumber,
            Name            = w.Name,
            AvailabilityStatus = w.AvailabilityStatus.ToString(),
            JobRoleName = w.JobRole == null ? null : w.JobRole.Name,
            SkillLevel = w.SkillLevel == null ? null : w.SkillLevel.ToString(),
        })
        .ToListAsync();


        return View(workersList);
    }

    public async Task<IActionResult> Create()
    {
        var userId = _userManager.GetUserId(User);
        var existingWorker = await _context.Workers.FirstOrDefaultAsync(w => w.ApplicationUserId == userId);
        if (existingWorker != null)
        {
            TempData["Info"] = "You already have a Worker profile.";

            return RedirectToAction(
                nameof(Details),
                new { id = existingWorker.Id });
        }

        var workerVM = new WorkerCreateViewModel
        {
            Departments = await _lookupService.PopulateDepartmentDropDownAsync(),
            JobRoles = await _lookupService.PopulateJobRoleDropDownAsync(),
        };

        return View(workerVM);
    }


    [HttpPost]
    public async Task<IActionResult> Create(WorkerCreateViewModel newWorker)
    {
        if (!ModelState.IsValid)
        {
            var workerVM = new WorkerCreateViewModel
            {
                Departments = await _lookupService.PopulateDepartmentDropDownAsync(),
                JobRoles = await _lookupService.PopulateJobRoleDropDownAsync(),
            };

            return View(workerVM);
        }

        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Challenge(); ///// dont know that is wha ??@

        bool exists = await _context.Workers
            .AnyAsync(w => w.ApplicationUserId == userId);

        if (exists)
        {
            ModelState.AddModelError(
                "",
                "Your account already has a Worker profile.");

            var workerVM = new WorkerCreateViewModel
            {
                Departments = await _lookupService.PopulateDepartmentDropDownAsync(),
                JobRoles = await _lookupService.PopulateJobRoleDropDownAsync(),
            };

            return View(workerVM);
        }


        var workerToAdd = new Worker
        {
            Name = newWorker.Name,
            PhoneNumber = newWorker.PhoneNumber,
            Email = newWorker.Email,
            DepartmentId = newWorker.DepartmentId,
            JobRoleId = newWorker.JobRoleId,
            SkillLevel = newWorker.SkillLevel,
            AvailabilityStatus = newWorker.AvailabilityStatus,

            ApplicationUserId = User.IsInRole(AppRoles.Worker) ? userId : null,
        };

        await _context.AddAsync(workerToAdd);
        await _context.SaveChangesAsync();

        workerToAdd.ReferenceNumber =
            GenerateWorkerReferenceNumber(workerToAdd.Id, workerToAdd.Department);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }    


    private static string GenerateReferenceNumber(string tag, int id, string? middleTag = null)
    {
        middleTag = string.IsNullOrEmpty(middleTag) ? DateTime.UtcNow.Year.ToString() : middleTag;

        return $"{tag}-{middleTag}-{id:D6}";
    }

    private static string GenerateWorkerReferenceNumber(int id, Department? department)
    {
        string name = department != null ? department.Name : "Unemployed";
        // WIT = Worker Identity Tag
        return GenerateReferenceNumber("WIT", id, name);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) {
            return BadRequest();
        }

        var workerToEdit = await _context.Workers.FindAsync(id);

        if (workerToEdit == null) {
            return NotFound();
        }

        if (!IsOwnerOrAdmin(workerToEdit.ApplicationUserId))
            return Forbid();

        var workerVM = new WorkerEditViewModel
        {
            Id = workerToEdit.Id,
            ReferenceNumber = workerToEdit.ReferenceNumber,
            Name = workerToEdit.Name,
            PhoneNumber = workerToEdit.PhoneNumber,
            Email = workerToEdit.Email,
            Departments = await _lookupService.PopulateDepartmentDropDownAsync(),
            JobRoles = await _lookupService.PopulateJobRoleDropDownAsync(),
            SkillLevel = workerToEdit.SkillLevel,
            AvailabilityStatus = workerToEdit.AvailabilityStatus,
            //Services = workerToEdit.Services,
        };

        return View(workerVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(WorkerEditViewModel workerVM)
    {
        if (!ModelState.IsValid)
        {
            workerVM.Departments = await _lookupService.PopulateDepartmentDropDownAsync();
            workerVM.JobRoles = await _lookupService.PopulateJobRoleDropDownAsync();
            return View(workerVM);
        }

        var workerToEdit = await _context.Workers.FindAsync(workerVM.Id);

        if (workerToEdit == null)
        {
            return NotFound();
        }

        if (!IsOwnerOrAdmin(workerToEdit.ApplicationUserId))
            return Forbid();

        workerToEdit.Name = workerVM.Name;
        workerToEdit.PhoneNumber = workerVM.PhoneNumber;
        workerToEdit.Email = workerVM.Email;
        workerToEdit.DepartmentId = workerVM.DepartmentId;
        workerToEdit.JobRoleId = workerVM.JobRoleId;
        workerToEdit.SkillLevel = workerVM.SkillLevel;
        workerToEdit.AvailabilityStatus = workerVM.AvailabilityStatus;
        //workerToEdit.Services = workerVM.Services;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new {id = workerToEdit.Id});
    }


    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return BadRequest();

        var worker = await _context.Workers
            .Include(w => w.Department)
            .Include(w => w.JobRole)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (worker == null)
            return NotFound();

        var workerVM = new WorkerViewModel
        {
            Id = worker.Id,
            ReferenceNumber = worker.ReferenceNumber,
            Name = worker.Name,
            AvailabilityStatus = worker.AvailabilityStatus.ToString(),
            PhoneNumber = worker.PhoneNumber,
            Email = worker.Email,
            DepartmentName = worker.Department?.Name,
            JobRoleName = worker.JobRole?.Name,
            SkillLevel = worker.SkillLevel.ToString(),
            Services = worker.Services,
        };

        return View(workerVM);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if(id == null)
            return BadRequest();

        var worker = await _context.Workers.FindAsync(id);

        if (worker == null)
            return NotFound();

        if (!IsOwnerOrAdmin(worker.ApplicationUserId))
            return Forbid();

        return View();
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var workerToDetele = await _context.Workers.FindAsync(id);
        if(workerToDetele == null) { return NotFound(); }
        if (!IsOwnerOrAdmin(workerToDetele.ApplicationUserId))
            return Forbid();

        //var deletedRows = await _context.Workers.Where(w => w.Id == id).ExecuteDeleteAsync();
        //if (deletedRows == 0)
        //{
        //    return NotFound();
        //}

        _context.Workers.Remove(workerToDetele);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool IsOwnerOrAdmin(string? ownerId)
    {
        var userId = _userManager.GetUserId(User);
        bool isAdmin = User.IsInRole(AppRoles.Administrator);
        bool isOwner = ownerId == userId;
        return isAdmin | isOwner;
    }
}
