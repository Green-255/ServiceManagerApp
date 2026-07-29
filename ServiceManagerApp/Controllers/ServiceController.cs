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
                Duration = newService.Duration,
                Location = newService.Location,
                //Workers = await GetWorkers(newService.WorkersIds),
                Comments = ParseComments(newService.Comments)
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

            var viewModel = new ServiceEditViewModel
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
        public async Task<IActionResult> Edit(ServiceEditViewModel serviceEdited)
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
    }
}
