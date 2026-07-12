using Microsoft.AspNetCore.Mvc;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

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
        return View(_context.ServiceRequests);
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
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var newRequest = new ServiceRequest
        {
            ServiceRequestType     = request.ServiceRequestType,
            Description     = request.Description,
            RequestedDueUtc = request.RequestedDueUtc, // GetDateTimeType(request.DueAt)
        };
        _context.ServiceRequests.Add(newRequest);
        _context.Services.Add(CreateServiceFromRequest(newRequest));

        await _context.SaveChangesAsync();

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
            ServiceRequestType = serviceRequest.ServiceRequestType,
            Title = serviceRequest.Title,
            Description = serviceRequest.Description,
            RequestStatus = serviceRequest.Status,
            CreatedAtUtc = serviceRequest.CreatedAtUtc,
            RequestedDueUtc = (DateTime) serviceRequest.RequestedDueUtc
        };

        return View();
    }
}
