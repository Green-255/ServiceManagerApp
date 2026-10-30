using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagerApp.Models.ViewModels.JobRoles
{
    public class JobRoleCreateEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Job role name is required.")]
        [StringLength(70, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(300, MinimumLength = 10)]
        public string? Description { get; set; }
        public int? DepartmentId { get; set; }
        public List<SelectListItem> Departments { get; set; } = [];
        //public ICollection<Worker> Workers { get; set; } = [];
    }
}
