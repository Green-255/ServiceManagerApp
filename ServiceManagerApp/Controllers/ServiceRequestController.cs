using Microsoft.AspNetCore.Mvc;
using ServiceManagerApp.Data;
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

    public async Task<IActionResult> Create(RequestType request)
    {
        var NewRequest = new ServiceRequest
        {
            ServiceType = (ServiceType) request.ServiceType,
            Description = request.Description,
            DueAt       = (DateTime) request.DueAt, // GetDateTimeType(request.DueAt)
        };
        _context.ServiceRequest.Add(NewRequest);
        await _context.SaveChangesAsync();

        CreateServiceFromRequest(NewRequest);

        return View();
    }


    private async CreateServiceFromRequest(ServiceRequest NewRequest)
    {
        var NewService = new Service
        {
            ServiceRequestId = NewRequest.Id,
            //ServiceRequestId = 
            ServiseType = NewRequest.ServiceType,
            ServiceStatus = ServiceStatus.Inactive,
            DueAt = NewRequest.DueAt,
            //Duration = 
            //Location = 
        };

        //string[] Comments = ParseServiceRequestComments(NewRequest.Description);
        //NewService.Comments = Comments;

        _context.Service.AddAsync(NewService);
        await _context.SaveChangesAsync();
    }
}
