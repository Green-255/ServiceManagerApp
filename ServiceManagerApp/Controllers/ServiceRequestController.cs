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
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateServiceRequestViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var newRequest = new ServiceRequest
        {
            ServiceType     = request.ServiceType,
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
            ServiceType    = newRequest.ServiceType,
            Status         = ServiceStatus.Draft,
            DueAtUtc       = newRequest.RequestedDueUtc,
            //Duration = 
            //Location = 
        };

        //string[] Comments = ParseServiceRequestComments(newRequest.Description);
        //newService.Comments = Comments;

        return newService;
    }
}
