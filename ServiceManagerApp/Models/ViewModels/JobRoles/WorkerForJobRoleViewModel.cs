using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.JobRoles
{
    public class WorkerForJobRoleViewModel
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty; // unique code for each worker, maybe use a GUID or a combination of name and ID
        public string Name { get; set; } = string.Empty;
        public string AvailabilityStatus { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
    }
}
