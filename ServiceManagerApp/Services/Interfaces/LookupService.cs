using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using ServiceManagerApp.Models.ViewModels.Departments;

namespace ServiceManagerApp.Services.Interfaces
{
    public class LookupService
    {
        private readonly ApplicationDbContext _context;

        public LookupService (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> PopulateDepartmentDropDownAsync()
        {
            return await _context.Departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            })
            .ToListAsync();
        }

        public async Task<List<SelectListItem>> PopulateJobRoleDropDownAsync()
        {
            return await _context.JobRoles.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            })
            .ToListAsync();
        }

       
        private static readonly ServiceStatus[] UnreviewedStatuses =
        {
            ServiceStatus.NeedsReview,
            ServiceStatus.Draft,
        };

        public async Task<List<Service>> PopulateServicesFromRequests()
        {
            return await _context.Services
                .Where(s => UnreviewedStatuses.Contains(s.Status))
                .ToListAsync();
        }

        public async Task<List<Service>> PopulateServicesFromRequests(IQueryable<Service> serviceQuery)
        {
            return await serviceQuery
                .Where(s => UnreviewedStatuses.Contains(s.Status))
                .ToListAsync(); // can return IQueryable instead of list
        }

        public async Task<List<JobRoleCheckboxViewModel>> GetJobRoleCheckboxesAsync(
            int? id = null,
            List<int>? selectedJobRoleIds = null)
        {
            selectedJobRoleIds ??= [];

            return await _context.JobRoles
                .Where(jr => jr.DepartmentId == null || jr.DepartmentId == id)
                .Select(jr => new JobRoleCheckboxViewModel
                {
                    Id = jr.Id,
                    Name = jr.Name,
                    IsSelected = selectedJobRoleIds.Contains(jr.Id)
                })
                .ToListAsync();
        }

    }
}
