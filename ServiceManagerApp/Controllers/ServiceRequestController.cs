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
    public async Task<IActionResult> Create(CreateServiceRequestViewModel request)
    {
        var NewRequest = new ServiceRequest
        {
            ServiceType = request.ServiceType,
            Description = request.Description,
            RequestedDueUtc = request.RequestedDueUtc, // GetDateTimeType(request.DueAt)
        };
        _context.ServiceRequests.Add(NewRequest);
        await _context.SaveChangesAsync();

        CreateServiceFromRequest(NewRequest);

        return View();
    }


    private async Task CreateServiceFromRequest(ServiceRequest NewRequest)
    {
        var NewService = new Service
        {
            ServiceRequestId = NewRequest.Id,
            ServiceRequest = NewRequest,
            ServiceType = NewRequest.ServiceType,
            Status = ServiceStatus.Inactive,
            DueAtUtc = NewRequest.RequestedDueUtc,
            //Duration = 
            //Location = 
        };

        //string[] Comments = ParseServiceRequestComments(NewRequest.Description);
        //NewService.Comments = Comments;

        _context.Services.Add(NewService);
        await _context.SaveChangesAsync();
    }
}
