using Microsoft.AspNetCore.Mvc.Rendering;
//using ServiceManagerApp.Models.Entities;

namespace ServiceManagerApp.Models.ViewModels.JobRoles
{
    public class JobRoleCreateEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? DepartmentId { get; set; }
        public List<SelectListItem> Departments { get; set; } = [];
        //public ICollection<Worker> Workers { get; set; } = [];
    }
}
