using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.ViewModels.Workers;

namespace ServiceManagerApp.Controllers
{
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public WorkerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var workersList = await _context.Workers.Select(w => new WorkerIndexViewModel
            {
                Id              = w.Id,
                ReferenceCode   = w.ReferenceCode,
                Name            = w.Name,
                Departament     = w.Departament,
                SkillLevel      = w.SkillLevel,
            })
            .ToListAsync();


            return View(workersList);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(WorkerCreateViewModel newWorker)
        {
            var workerToAdd = new Worker
            {
                Id = newWorker.Id,
                Name = newWorker.Name,
                PhoneNumber = newWorker.PhoneNumber,
                Email = newWorker.Email,
                Departament = newWorker.Departament,
                JobRole = newWorker.JobRole,
                SkillLevel = newWorker.SkillLevel,
            };

            await _context.AddAsync(workerToAdd);
            await _context.SaveChangesAsync();

            workerToAdd.ReferenceNumber = GenerateWorkerReferenceNumber(workerToAdd.Id);
        }

        private static string GenerateReferenceNumber(string tag, int id, string? middleTag = null)
        {
            middleTag = string.IsNullOrEmpty(middleTag) ? DateTime.UtcNow.Year.ToString() : middleTag;

            return $"{tag}-{middleTag}-{id:D6}";
        }

        private static string GenerateWorkerReferenceNumber(int id)
        {
            // WIT = Worker Identity Tag
            return GenerateReferenceNumber("WIT", id);
        }
    }
}
