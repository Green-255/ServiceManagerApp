using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using ServiceManagerApp.Models.ViewModels.ServiceRequests;

namespace ServiceManagerApp.Controllers;

public class ServiceRequestController : Controller
{
    private readonly ApplicationDbContext _context;
    public ServiceRequestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var serviceRequests = await _context.ServiceRequests
            .OrderByDescending(sr => sr.CreatedAtUtc).ToListAsync();

        var serviceRequestsList = new List<ServiceRequestIndexViewModel>();

        foreach (var sr in serviceRequests)
        {
            var srViewModel = new ServiceRequestIndexViewModel
            {
                Id = sr.Id,
                ServiceRequestType = sr.ServiceRequestType,
                Title = sr.Title,
                Description = sr.Description,
                Status = sr.Status,
                RequestedDueUtc = sr.RequestedDueUtc,
                ReferenceNumber = sr.ReferenceNumber
            };

            serviceRequestsList.Add(srViewModel);
        };

        return View(serviceRequestsList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceRequestCreateViewModel request)
    {
        var createdAtUtc = DateTime.UtcNow;
        
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var newRequest = new ServiceRequest
        {
            ServiceRequestType = request.ServiceRequestType,
            Title           = request.Title,
            Description     = request.Description,
            RequestedDueUtc = request.RequestedDueUtc, // GetDateTimeType(request.DueAt)
            CreatedAtUtc    = createdAtUtc
        };
        _context.ServiceRequests.Add(newRequest);
        _context.Services.Add(CreateServiceFromRequest(newRequest));

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Cancel()
    {
        return RedirectToAction(nameof(Index));
    }


    private Service CreateServiceFromRequest(ServiceRequest newRequest)
    {
        var newService = new Service
        {
            //ServiceRequestId = newRequest.Id, // Id is not created, 'cause Changes not saved yet.
            ServiceRequest = newRequest,
            ServiceRequestType = newRequest.ServiceRequestType,
            Status         = ServiceStatus.Draft,
            DueAtUtc       = newRequest.RequestedDueUtc,
            //Duration = 
            //Location = 
        };

        //string[] Comments = ParseServiceRequestComments(newRequest.Description);
        //newService.Comments = Comments;

        return newService;
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(id);

        if (serviceRequest == null)
        {
            return NotFound();
        }

        var serviceRequestDetailsViewModel = new ServiceRequestDetailsViewModel
        {
            Id = id,
            ServiceRequestType = serviceRequest.ServiceRequestType,
            Title = serviceRequest.Title,
            Description = serviceRequest.Description,
            RequestStatus = serviceRequest.Status,
            CreatedAtUtc = serviceRequest.CreatedAtUtc,
            RequestedDueUtc = serviceRequest.RequestedDueUtc 
        };

        return View(serviceRequestDetailsViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var serviceRequestToDetele = await _context.ServiceRequests.FindAsync(id);
        if(serviceRequestToDetele == null)
        {
            //return NotFound();
            return RedirectToAction(nameof(Index));
        }
        _context.ServiceRequests.Remove(serviceRequestToDetele);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
