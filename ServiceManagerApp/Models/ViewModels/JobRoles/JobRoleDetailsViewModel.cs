using ServiceManagerApp.Models.ViewModels;

namespace ServiceManagerApp.Models.ViewModels.JobRoles
{
    public class JobRoleDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public List<WorkerForJobRoleViewModel> Workers { get; set; } = [];
    }
}
