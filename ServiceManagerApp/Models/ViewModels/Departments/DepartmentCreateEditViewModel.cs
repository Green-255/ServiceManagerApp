using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceManagerApp.Models.Entities;

namespace ServiceManagerApp.Models.ViewModels.Departments
{
    public class DepartmentCreateEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<int> JobRoleIds { get; set; } = [];
        public List<SelectListItem> JobRoles { get; set; } = [];
    }
}
