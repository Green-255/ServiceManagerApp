using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Authorization;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using ServiceManagerApp.Models.ViewModels.Services;
using ServiceManagerApp.Models.ViewModels.Services.Review;
using ServiceManagerApp.Utilities;

namespace ServiceManagerApp.Controllers;

[Authorize(Roles = AppRoles.Administrator + "," + AppRoles.Worker)]
public class ServiceController : Controller
{
    private readonly ApplicationDbContext _context;
    private static readonly ServiceStatus[] UnreviewedStatuses =
        {
            ServiceStatus.NeedsReview,
            ServiceStatus.Draft,
        };

    public ServiceController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var services = await _context.Services
            .AsNoTracking()
            .OrderByDescending(s => s.Priority)
            .ThenBy(s => s.DueAtUtc)
            .Select(s => new ServiceIndexListViewModel
            {
                Id = s.Id,
                ServiceRequest = s.ServiceRequest!,
                ReferenceNumber = s.ReferenceNumber,
                Status = s.Status,
                DueAtUtc = s.DueAtUtc,
                Priority = s.Priority,
            })
            .ToListAsync();

        var allServices = new List<ServiceIndexListViewModel>();
        var needReviewServices = new List<ServiceIndexListViewModel>();

        foreach (var service in services)
        {

            if (UnreviewedStatuses.Contains(service.Status)){
                needReviewServices.Add(service);
            }
            else
            {
                allServices.Add(service);
            }
        }

        var servicesViewModel = new ServiceIndexViewModel
        {
            AllServices = allServices,
            ServicesNeedingReview = needReviewServices
        };

        return View(servicesViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var service = new ServiceCreateViewModel
        {
            Workers = await _context.Workers
            .Select(w => new ServiceReviewWorkerViewModel
            {
                Id = w.Id,
                Name = w.Name,
                SkillLevel = w.SkillLevel.ToString()!,
                IsSelected = false
            })
            .ToListAsync()
        };

        return View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceCreateViewModel newService)
    {
        if (!ModelState.IsValid)
        {
            return View(newService);
        }

        var workers = await _context.Workers
            .Where(w => newService.WorkerIds.Contains(w.Id))
            .ToListAsync();

        var serviceToAdd = new Service
        {
            ServiceRequest = newService.ServiceRequest,
            Status = ServiceStatus.Draft,
            DueAtUtc = newService.DueAtUtc,
            Duration = DateTimeHelper.CalculateDuration(newService.DurationHours, newService.DurationMinutes),
            Location = newService.Location,
            Priority = newService.Priority,
            Workers  = workers,
            SkillLevel = newService.SkillLevel,
            Description = newService.Description,
            Cost     = (float) newService.Cost,
        };

        _context.Services.Add(serviceToAdd);
        await _context.SaveChangesAsync();

        serviceToAdd.ReferenceNumber = ReferenceNumberGenerator.Generate("WO", serviceToAdd.Id);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var serviceToEdit = await _context.Services
            .Include(s => s.Workers)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (serviceToEdit == null)
        {
            return NotFound();
        }

        var assignedWorkerIds = serviceToEdit.Workers.Select(sw => sw.Id).ToList();
        var workersAssignStatus = await _context.Workers
            .Select(w => new ServiceReviewWorkerViewModel
            {
                Id = w.Id,
                Name = w.Name,
                SkillLevel = w.SkillLevel.ToString()!,
                IsSelected = assignedWorkerIds.Contains(w.Id)
            })
            .ToListAsync();

        var viewModel = new ServiceEditViewModel
        {
            Id = id,
            ServiceRequest = serviceToEdit.ServiceRequest,
            ReferenceNumber = serviceToEdit.ReferenceNumber,
            Status = serviceToEdit.Status,
            DueAtUtc = serviceToEdit.DueAtUtc,
            Location = serviceToEdit.Location,
            Priority = serviceToEdit.Priority,
            WorkerIds = assignedWorkerIds,
            Workers = workersAssignStatus,
            SkillLevel = serviceToEdit.SkillLevel,
            Description = serviceToEdit.Description,
            Cost = serviceToEdit.Cost,
        };

        if (serviceToEdit.Duration != null)
        {
            viewModel.DurationHours = serviceToEdit.Duration.Value.Hours;
            viewModel.DurationMinutes = serviceToEdit.Duration.Value.Minutes;
        }

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ServiceEditViewModel serviceEdited)
    {
        if (!ModelState.IsValid)
        {
            return View(serviceEdited);
        }

        var serviceToUpdate = await _context.Services
                    .Include(s => s.Workers)
                    .FirstOrDefaultAsync(s => s.Id == serviceEdited.Id);

        if (serviceToUpdate == null)
        {
            return NotFound();
        }

        var workers = await _context.Workers
            .Where(w => serviceEdited.WorkerIds.Contains(w.Id))
            .ToListAsync();

        serviceToUpdate.ServiceRequest = serviceEdited.ServiceRequest;
        serviceToUpdate.Status = serviceEdited.Status;
        serviceToUpdate.DueAtUtc = serviceEdited.DueAtUtc;
        serviceToUpdate.Duration = DateTimeHelper.CalculateDuration(
                serviceEdited.DurationHours, serviceEdited.DurationMinutes);
        serviceToUpdate.Priority = serviceEdited.Priority;
        serviceToUpdate.Location = serviceEdited.Location;
        serviceToUpdate.Workers = workers;
        serviceToUpdate.SkillLevel = serviceEdited.SkillLevel;
        serviceToUpdate.Description = serviceEdited.Description;
        serviceToUpdate.Cost = serviceEdited.Cost;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new {id = serviceToUpdate.Id});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> delete(int? id)
    {
        if(id == null)
        {
            return BadRequest();
        }

        var serviceToRemove = await _context.Services.FindAsync(id);
        if(serviceToRemove == null)
        {
            return NotFound();
        }

        _context.Services.Remove(serviceToRemove);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if(id == null)
        {
            return BadRequest();
        }

        var service = await _context.Services.FindAsync(id);
            //.Include(s => s.ServiceRequest)
            //.Include(s => s.Workers)
            //.FirstOrDefaultAsync(s => s.Id == id);
        if (service == null)
        {
            return NotFound();
        }
        var viewModel = new ServiceDetailsViewModel
        {
            Id = service.Id,
            ReferenceNumber = service.ReferenceNumber,
            Status = service.Status,
            DueAtUtc = service.DueAtUtc,
            Priority = service.Priority,
            Duration = service.Duration,
            Location = service.Location,
            SkillLevel = service.SkillLevel?.ToString() ?? "Not set",
            Description = service.Description,
            Cost = service.Cost
        };
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Review(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var service = await _context.Services
            .Include(s => s.ServiceRequest)
            .Include(s => s.Workers)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (service == null)
        {
            return NotFound();
        }

        var serviceVM = await PopulateServiceRequestReviewViewModel(service);

        if (serviceVM == null)
        {
            return NotFound();
        }

        return View(serviceVM);
    }

    [HttpPost, ActionName("Review")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReviewAccepted(ServiceReviewViewModel serviceVM)
    {
        if (!ModelState.IsValid)
        {
            var service1 = await _context.Services
            .Include(s => s.ServiceRequest)
            .Include(s => s.Workers)
            .FirstOrDefaultAsync(s => s.Id == serviceVM.Id);

            if (service1 == null)
            {
                return NotFound();
            }
            serviceVM = await PopulateServiceRequestReviewViewModel(service1);

            serviceVM.Workers = await _context.Workers
                .Where(w => w.AvailabilityStatus == AvailabilityStatus.Available)
                .Select(w => new ServiceReviewWorkerViewModel
                {
                    Id = w.Id,
                    Name = w.Name,
                    SkillLevel = w.SkillLevel.ToString()!,
                    IsSelected = serviceVM.WorkerIds.Contains(w.Id)
                })
                .ToListAsync();

            return View("Review", serviceVM);
        }

        var service = await _context.Services
            .Include(s => s.ServiceRequest)
            .Include(s => s.Workers)
            .FirstOrDefaultAsync(s => s.Id == serviceVM.Id);

        if (service == null)
        {
            return NotFound();
        }

        service.DueAtUtc = serviceVM.DueAtUtc;
        service.Priority = serviceVM.Priority;
        service.Duration = DateTimeHelper.CalculateDuration(serviceVM.DurationHours, serviceVM.DurationMinutes);
        service.Location = serviceVM.Location;
        service.SkillLevel = serviceVM.SkillLevel;
        service.Description = serviceVM.Description;
        service.Cost = serviceVM.Cost;

        service.Workers = await _context.Workers
            .Where(w => serviceVM.WorkerIds.Contains(w.Id))
            .ToListAsync();

        service.Status = ServiceStatus.Scheduled;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReviewRejected(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var serviceToReject = await _context.Services
            .Include(s => s.ServiceRequest)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (serviceToReject == null)
        {
            return NotFound();
        }

        serviceToReject.Status = ServiceStatus.Cancelled;

        if(serviceToReject.ServiceRequest != null)
        {
            serviceToReject.ServiceRequest.Status = ServiceRequestStatus.Rejected;
        }

        await _context.SaveChangesAsync();
    
        return RedirectToAction(nameof(Index));
    }


    private async Task<ServiceReviewViewModel> PopulateServiceRequestReviewViewModel(Service service)
    {
        var assignedWorkerIds = service.Workers
            .Select(sw => sw.Id)
            .ToList();

        var workersAvailable = await _context.Workers
            .Select(w => new ServiceReviewWorkerViewModel
            {
                Id = w.Id,
                Name = w.Name,
                SkillLevel = w.SkillLevel.HasValue
                    ? w.SkillLevel.Value.ToString()
                    : "Not set",

                Status = w.AvailabilityStatus.ToString(),
                IsSelected = assignedWorkerIds.Contains(w.Id)
            })
            .ToListAsync();


        int minutes = DateTimeHelper.GetMinutesFromTimeSpan(service.Duration);

        var serviceVM = new ServiceReviewViewModel
        {
            Id = service.Id,
            ServiceRequest = service.ServiceRequest == null
            ? null
            : new ServiceRequestDetailsViewModel
            {
                Id = service.ServiceRequest.Id,
                ReferenceNumber = service.ServiceRequest.ReferenceNumber,
                Title = service.ServiceRequest.Title,
            },
            ReferenceNumber = service.ReferenceNumber,
            DueAtUtc = service.DueAtUtc,
            DurationHours = minutes / 60,
            DurationMinutes = minutes % 60,
            Priority = service.Priority,
            Location = service.Location,
            Workers = workersAvailable,
            SkillLevel = service.SkillLevel,
            Description = service.Description,
            Cost = service.Cost,
        };

        return serviceVM;
    }
}
