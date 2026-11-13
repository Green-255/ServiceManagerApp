using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Authorization;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using ServiceManagerApp.Models.ViewModels.ServiceRequests;
using System.Net;

namespace ServiceManagerApp.Controllers;

[Authorize(Roles = AppRoles.Customer + "," + AppRoles.Administrator)]
public class ServiceRequestController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ServiceRequestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);

        IQueryable<ServiceRequest> query = _context.ServiceRequests.AsNoTracking();

        if (User.IsInRole(AppRoles.Customer) && userId != null)
        {
            query = query.Where(sr => sr.CustomerUserId == userId);
        }
        else if (!User.IsInRole(AppRoles.Administrator))
        {
            return View(new List<ServiceRequestIndexViewModel>());
        }

        var serviceRequestsList = await query
            .OrderByDescending(sr => sr.CreatedAtUtc)
            .Select(sr => new ServiceRequestIndexViewModel
            {
                Id = sr.Id,
                ServiceRequestType = sr.ServiceRequestType,
                Title = sr.Title,
                Description = sr.Description,
                Status = sr.Status,
                RequestedDueUtc = sr.RequestedDueUtc,
            })
            .ToListAsync();

        return View(serviceRequestsList);
    }

    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceRequestCreateViewModel request)
    {
        if (!ModelState.IsValid)
            return View(request);
        
        var createdAtUtc = DateTime.UtcNow;

        var newRequest = new ServiceRequest
        {
            ServiceRequestType = request.ServiceRequestType,
            Title           = request.Title,
            Description     = request.Description,
            RequestedDueUtc = request.RequestedDueUtc, // GetDateTimeType(request.DueAt)
            CreatedAtUtc    = createdAtUtc,

            CustomerUserId = _userManager.GetUserId(User)
        };

        var newService = CreateServiceFromRequest(newRequest);

        _context.ServiceRequests.Add(newRequest);
        _context.Services.Add(newService);

        await _context.SaveChangesAsync();
        
        newRequest.ReferenceNumber = GenerateServiceRequestReferenceNumber(newRequest.Id);
        newService.ReferenceNumber = GenerateServiceReferenceNumber(newService.Id);
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

    private static string GenerateReferenceNumber(string tag, int id)
    {
        return $"{tag}-{DateTime.UtcNow.Year}-{id:D6}";
    }

    private static string GenerateServiceReferenceNumber(int id)
    {
        // WO = Work Order
        return GenerateReferenceNumber("WO", id);
    }

    private static string GenerateServiceRequestReferenceNumber(int id)
    {
        // REQ = Request
        return GenerateReferenceNumber("REQ", id);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(id);

        if (serviceRequest == null)
            return NotFound();

        if (!IsOwnerOrAdmin(serviceRequest.CustomerUserId))
            return Forbid(); // or NotFound() to avoid disclosing that another customer's record exists.

        var serviceRequestDetailsViewModel = new ServiceRequestDetailsViewModel
        {
            Id                 = id,
            ServiceRequestType = serviceRequest.ServiceRequestType,
            Title              = serviceRequest.Title,
            Description        = serviceRequest.Description,
            RequestStatus      = serviceRequest.Status,
            CreatedAtUtc       = serviceRequest.CreatedAtUtc,
            RequestedDueUtc    = serviceRequest.RequestedDueUtc 
        };

        return View(serviceRequestDetailsViewModel);
    }   

    [HttpGet]
    public async Task<IActionResult> Delete (int? id)
    {
        if (id == null)
            return BadRequest();

        var serviceRequest = await _context.ServiceRequests.FindAsync(id);

        if (serviceRequest == null)
            return NotFound();

        if (!IsOwnerOrAdmin(serviceRequest.CustomerUserId))
            return Forbid();

        return View(serviceRequest);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var serviceRequestToDetele = await _context.ServiceRequests.FindAsync(id);

        if (serviceRequestToDetele == null)
            return NotFound();
            //return RedirectToAction(nameof(Index));

        if (!IsOwnerOrAdmin(serviceRequestToDetele.CustomerUserId))
            return Forbid();

        _context.ServiceRequests.Remove(serviceRequestToDetele);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Edit (int id)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(id);
        if (serviceRequest == null)
            return NotFound();

        if (!IsOwnerOrAdmin(serviceRequest.CustomerUserId))
            return Forbid();

        var serviceRequestEditViewModel = new ServiceRequestEditViewModel
        {
            Id                 = serviceRequest.Id,
            ReferenceNumber    = serviceRequest.ReferenceNumber,
            ServiceRequestType = serviceRequest.ServiceRequestType,
            Title              = serviceRequest.Title,
            Description        = serviceRequest.Description,
            RequestedDueUtc    = serviceRequest.RequestedDueUtc,
            //Status = ServiceRequestStatus.Pending,
        };
        return View(serviceRequestEditViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit (ServiceRequestEditViewModel newRequest)
    {
        if (!ModelState.IsValid)
            return View(newRequest);

        var requestToUpdate = await _context.ServiceRequests.FindAsync(newRequest.Id);
        if (requestToUpdate == null)
            return NotFound();

        if (!IsOwnerOrAdmin(requestToUpdate.CustomerUserId))
            return Forbid();

        requestToUpdate.ServiceRequestType = newRequest.ServiceRequestType;
        requestToUpdate.Title              = newRequest.Title;
        requestToUpdate.Description        = newRequest.Description;
        requestToUpdate.RequestedDueUtc    = newRequest.RequestedDueUtc;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new {id = requestToUpdate.Id});
    }

    private bool IsOwnerOrAdmin(string? CustomerId)
    {
        var userId = _userManager.GetUserId(User);
        bool IsOwner = CustomerId == userId;
        bool IsAdmin = User.IsInRole(AppRoles.Administrator);
        return IsOwner || IsAdmin;
    }
}
