
namespace ServiceManagerApp.Models.ViewModels.Departments
{
    public record JobRoleSummaryItem(string RoleName, int WorkersCount);
    public class DepartmentDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int NumberOfJobRoles { get; set; } = 0;
        public List<JobRoleSummaryItem> JobRoleSummaries { get; set; } = [];
    }
}
