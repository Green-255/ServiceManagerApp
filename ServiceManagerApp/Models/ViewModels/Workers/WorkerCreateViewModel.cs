using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.Workers
{
    public class WorkerCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
        public int? DepartmentId { get; set; }
        public List<SelectListItem> Departments { get; set; } = [];
        public int? JobRoleId { get; set; }
        public List<SelectListItem> JobRoles { get; set; } = [];
        public SkillLevel? SkillLevel { get; set; }
    }
}
