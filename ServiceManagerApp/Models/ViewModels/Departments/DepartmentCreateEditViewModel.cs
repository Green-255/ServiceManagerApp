using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceManagerApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagerApp.Models.ViewModels.Departments
{
    public class DepartmentCreateEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(70, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength (300, MinimumLength = 10)]
        public string? Description { get; set; }
        public List<int> JobRoleIds { get; set; } = [];
        public List<JobRoleCheckboxViewModel> JobRoles { get; set; } = [];
    }
}
