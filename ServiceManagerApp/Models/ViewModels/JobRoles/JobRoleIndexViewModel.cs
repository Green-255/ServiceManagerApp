using ServiceManagerApp.Models.Entities;

namespace ServiceManagerApp.Models.ViewModels.JobRoles
{
    public class JobRoleIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int NumberOfWorkers { get; set; } = 0;
    }
}
