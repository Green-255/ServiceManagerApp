using Microsoft.AspNetCore.Mvc;
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
            var servicesList = PopulateServiceList();
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

        public async Task<IActionResult> Edit(Service serviceToEdit)
        {
            return View(serviceToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceCreateViewModel unhandledRequest)
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IndexFromRequest()
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
        public async Task<IActionResult> delete(Service serviceToDelete)
        {
            _context.Services.Remove(serviceToDelete);
            await _context.SaveChangesAsync();
            return View(); // redirect to index, cause Services are listed there?
        }

        private List<Service> PopulateServiceList()
        {
            var services = _context.Services.ToList();
            return services;
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
            return _context.Services
                .Where(s => s.Status == ServiceStatus.InProgress)
                .ToList();
        }
    }
}
