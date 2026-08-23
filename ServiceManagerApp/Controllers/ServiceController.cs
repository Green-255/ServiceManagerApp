using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using ServiceManagerApp.Models.ViewModels.Services;

namespace ServiceManagerApp.Controllers;
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
    public async Task<IActionResult> Index()
    {
        var services = await _context.Services
            .OrderByDescending(s => s.DueAtUtc).ToListAsync();
        var serviceRequests = await _context.ServiceRequests
            .OrderByDescending(sr => sr.CreatedAtUtc).ToListAsync();


        var allServices = new List<ServiceIndexListViewModel>();
        var needReviewServices = new List<ServiceIndexListViewModel>();

        foreach (var service in services)
        {
            var s = new ServiceIndexListViewModel
            {
                Id = service.Id,
                ServiceRequest = service.ServiceRequest,
                ReferenceNumber = service.ReferenceNumber,
                ServiceRequestType = service.ServiceRequestType,
                Status = service.Status,
                DueAtUtc = service.DueAtUtc,
            };

            allServices.Add(s);

            if (UnreviewedStatuses.Contains(s.Status)){
                needReviewServices.Add(s);
            }
        }

        // WHICH APPROACH IS BETTER?

        //var allServices = await _context.Services
        //    .OrderByDescending(s => s.DueAtUtc)
        //    .Select(s => new ServiceListItemViewModel
        //    {
        //        Id = s.Id,
        //        ServiceType = s.ServiceType,
        //        Status = s.Status,
        //        DueAtUtc = s.DueAtUtc,
        //        Duration = s.Duration,
        //        Location = s.Location
        //    })
        //    .ToListAsync(); 

        //var servicesNeedingReview = await _context.Services
        //    .Where(s => reviewStatuses.Contains(s.Status))
        //    .OrderByDescending(s => s.DueAtUtc)
        //    .Select(s => new ServiceListItemViewModel
        //    {
        //        Id = s.Id,
        //        ServiceType = s.ServiceType,
        //        Status = s.Status,
        //        DueAtUtc = s.DueAtUtc,
        //        Duration = s.Duration,
        //        Location = s.Location
        //    })
        //    .ToListAsync(); 

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
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ServiceCreateViewModel newService)
    {
        var serviceToAdd = new Service
        {
            ServiceRequest = newService.ServiceRequest,
            ServiceRequestType = newService.ServiceRequestType,
            Status = ServiceStatus.NeedsReview,
            DueAtUtc = newService.DueAtUtc,
            Duration = CalculateDuration(newService.DurationHours, newService.DurationMinutes),
            Location = newService.Location,
            //Workers = await GetWorkers(newService.WorkersIds),
            Comments = ParseComments(newService.Comments),
            Cost     = newService.Cost,
        };

        _context.Services.Add(serviceToAdd);
        await _context.SaveChangesAsync();

        serviceToAdd.ReferenceNumber = GenerateServiceReferenceNumber(serviceToAdd.Id);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private static string GenerateReferenceNumber(string tag, int id)
    {
        return $"{tag}-{DateTime.UtcNow.Year}-{id:D6}";
    }

    private static string GenerateServiceReferenceNumber(int id)
    {
        // WO = Work Order
        return GenerateReferenceNumber("WO", id);
    }

    private TimeSpan CalculateDuration(int? hours, int? minutes)
    {
        int totalMinutes = (hours ?? 0) * 60 + (minutes ?? 0);
        return TimeSpan.FromMinutes(totalMinutes);
    }

    private int GetMinutesFromTimeSpan(TimeSpan duration)
    {
        //if (duration == null) return 0;
        int minutes = duration.Hours * 60 + duration.Minutes;
        return minutes;
    }

    public async Task<IActionResult> Edit(int id)
    {
        var serviceToEdit = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

        if (serviceToEdit == null)
        {
            return NotFound();
        }

        var viewModel = new ServiceEditViewModel
        {
            Id = id,
            ServiceRequest = serviceToEdit.ServiceRequest,
            ServiceRequestType = serviceToEdit.ServiceRequestType,
            Status = serviceToEdit.Status,
            DueAtUtc = serviceToEdit.DueAtUtc,
            Location = serviceToEdit.Location,
            Workers = serviceToEdit.Workers,
            Comments = serviceToEdit.Comments,
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
    public async Task<IActionResult> Edit(ServiceEditViewModel serviceEdited)
    {
        if (!ModelState.IsValid)
        {
            return View(serviceEdited);
        }

        //var serviceToUpdate = await _context.Services
        //    .FirstOrDefaultAsync(s => s.Id == serviceEdited.Id);
        var serviceToUpdate = await _context.Services.FindAsync(serviceEdited.Id);

        if (serviceToUpdate == null)
        {
            return NotFound();
        }

        serviceToUpdate.ServiceRequest = serviceEdited.ServiceRequest;
        serviceToUpdate.ServiceRequestType = serviceEdited.ServiceRequestType;
        serviceToUpdate.Status = serviceEdited.Status;
        serviceToUpdate.DueAtUtc = serviceEdited.DueAtUtc;
        serviceToUpdate.Duration = CalculateDuration(serviceEdited.DurationHours, serviceEdited.DurationMinutes);
        serviceToUpdate.Location = serviceEdited.Location;
        serviceToUpdate.Workers = serviceEdited.Workers;
        serviceToUpdate.Comments = serviceEdited.Comments;
        serviceToUpdate.Cost = serviceEdited.Cost;

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> NeedsReview()
    {
        var serviceList = await PopulateServicesFromRequests();
        return View(serviceList);
    }

    [HttpPost]
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

    private async Task<List<Service>> PopulateServicesFromRequests()
    {
        return await _context.Services
            .Where(s => UnreviewedStatuses.Contains(s.Status))
            .ToListAsync();
    }

    private async Task<List<Service>> PopulateServicesInProgress()
    {
        return await _context.Services
            .Where(s => s.Status == ServiceStatus.InProgress)
            .ToListAsync();
    }

    private string[] ParseComments(string commentsToParse)
    {
        string[] comments = commentsToParse.Split(".");

        return comments;
    }


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
            ServiceRequestType = service.ServiceRequestType,
            Status = service.Status,
            DueAtUtc = service.DueAtUtc,
            Duration = service.Duration,
            Location = service.Location,
            Cost = service.Cost
        };
        return View(viewModel);
    }
}
