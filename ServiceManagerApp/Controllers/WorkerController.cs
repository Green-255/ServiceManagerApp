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
                ReferenceNumber = w.ReferenceNumber,
                Name            = w.Name,
                JobRole         = w.JobRole,
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
                Department = newWorker.Department,
                JobRole = newWorker.JobRole,
                SkillLevel = newWorker.SkillLevel,
            };

            await _context.AddAsync(workerToAdd);
            await _context.SaveChangesAsync();

            workerToAdd.ReferenceNumber =
                GenerateWorkerReferenceNumber(workerToAdd.Id, workerToAdd.Department);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private static string GenerateReferenceNumber(string tag, int id, string? middleTag = null)
        {
            middleTag = string.IsNullOrEmpty(middleTag) ? DateTime.UtcNow.Year.ToString() : middleTag;

            return $"{tag}-{middleTag}-{id:D6}";
        }

        private static string GenerateWorkerReferenceNumber(int id, Department? department)
        {
            string name = department != null ? department.Name : "Unemployed";
            // WIT = Worker Identity Tag
            return GenerateReferenceNumber("WIT", id, name);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) {
                return BadRequest();
            }

            var workerToEdit = await _context.Workers.FindAsync(id);

            if (workerToEdit == null) {
                return NotFound();
            }

            var workerVM = new WorkerViewModel
            {
                Id = workerToEdit.Id,
                ReferenceNumber = workerToEdit.ReferenceNumber,
                Name = workerToEdit.Name,
                AvailabilityStatus = workerToEdit.AvailabilityStatus,
                PhoneNumber = workerToEdit.PhoneNumber,
                Email = workerToEdit.Email,
                Department = workerToEdit.Department,
                JobRole = workerToEdit.JobRole,
                SkillLevel = workerToEdit.SkillLevel,
                Services = workerToEdit.Services,
            };

            return View(workerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorkerViewModel workerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(workerVM);
            }

            var workerToEdit = await _context.Workers.FindAsync(workerVM.Id);

            if (workerToEdit == null)
            {
                return NotFound();
            }

            workerToEdit.Name = workerVM.Name;
            workerToEdit.AvailabilityStatus = workerVM.AvailabilityStatus;
            workerToEdit.PhoneNumber = workerVM.PhoneNumber;
            workerToEdit.Email = workerVM.Email;
            workerToEdit.Department = workerVM.Department;
            workerToEdit.JobRole = workerVM.JobRole;
            workerToEdit.SkillLevel = workerVM.SkillLevel;
            workerToEdit.Services = workerVM.Services;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var worker = await _context.Workers.FindAsync(id);

            if (worker == null)
            {
                return NotFound();
            }

            var workerVM = new WorkerViewModel
            {
                Id = worker.Id,
                ReferenceNumber = worker.ReferenceNumber,
                Name = worker.Name,
                AvailabilityStatus = worker.AvailabilityStatus,
                PhoneNumber = worker.PhoneNumber,
                Email = worker.Email,
                Department = worker.Department,
                JobRole = worker.JobRole,
                SkillLevel = worker.SkillLevel,
                Services = worker.Services,
            };

            return View(workerVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deletedRows = await _context.Workers.Where(w => w.Id == id).ExecuteDeleteAsync();
            if (deletedRows == 0)
            {
                return NotFound();
            }

            //var workerToDetele = _context.Workers.Find(id);
            //if (workerToDetele == null)
            //{
            //    return NotFound();
            //}
            //_context.Workers.Remove(workerToDetele);
            //await _context.SaveChangesAsync();

            //retu1`rn Ok(deletedRows);
            return RedirectToAction(nameof(Index));
        }
    }
}
