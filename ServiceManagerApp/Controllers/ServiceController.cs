using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var servicesList = await _context.Services.OrderByDescending(s => s.DueAtUtc).ToListAsync();
            return View(servicesList);
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
                Status = newService.Status,
                DueAtUtc = newService.DueAtUtc,
                Duration = newService.Duration,
                Location = newService.Location,
                Workers = newService.Workers,
                Comments = newService.Comments
            };

            _context.Services.Add(serviceToAdd);
            await _context.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var serviceToEdit = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

            if (serviceToEdit == null)
            {
                return NotFound();
            }

            var viewModel = new ServiceCreateViewModel
            {
                Id = id,
                ServiceRequest = serviceToEdit.ServiceRequest,
                ServiceRequestType = serviceToEdit.ServiceRequestType,
                Status = serviceToEdit.Status,
                DueAtUtc = serviceToEdit.DueAtUtc,
                Duration = serviceToEdit.Duration,
                Location = serviceToEdit.Location,
                Workers = serviceToEdit.Workers,
                Comments = serviceToEdit.Comments
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceCreateViewModel serviceEdited)
        {
            if (!ModelState.IsValid)
            {
                return View(serviceEdited);
            }

            var serviceToUpdate = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceEdited.Id);

            if(serviceToUpdate == null)
            {
                return NotFound();
            }

            serviceToUpdate.ServiceRequest = serviceEdited.ServiceRequest;
            serviceToUpdate.ServiceRequestType = serviceEdited.ServiceRequestType;
            serviceToUpdate.Status = serviceEdited.Status;
            serviceToUpdate.DueAtUtc = serviceEdited.DueAtUtc;
            serviceToUpdate.Duration = serviceEdited.Duration;
            serviceToUpdate.Location = serviceEdited.Location;
            serviceToUpdate.Workers = serviceEdited.Workers;
            serviceToUpdate.Comments = serviceEdited.Comments;

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
        public async Task<IActionResult> EditFromRequest(HandleRequestViewModel unhandledRequest)
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> delete(int id)
        {
            var serviceToRemove = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if(serviceToRemove == null)
            {
                return NotFound();
            }

            _context.Services.Remove(serviceToRemove);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private async Task<List<Service>> PopulateServicesFromRequests()
        {
            var unreviewedStatuses = new[]
            {
                ServiceStatus.NeedsReview,
                ServiceStatus.Draft,
            };

            return await _context.Services
                .Where(s => unreviewedStatuses.Contains(s.Status))
                .ToListAsync();
        }

        private async Task<List<Service>> PopulateServicesInProgress()
        {
            return await _context.Services
                .Where(s => s.Status == ServiceStatus.InProgress)
                .ToListAsync();
        }
    }
}
