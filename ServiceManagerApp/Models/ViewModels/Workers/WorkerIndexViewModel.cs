using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.Workers
{
    public class WorkerIndexViewModel
    {
        public int Id { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.Available;
        //public string? PhoneNumber { get; set; }
        //public string Email { get; set; } = null!;
        //public int? DepartamentId { get; set; }
        public Departament? Departament { get; set; }
        //public int? JobRoleId { get; set; }
        //public JobRole? JobRole { get; set; }
        public SkillLevel? SkillLevel { get; set; }
        //public ICollection<Service> Services { get; set; } = [];
    }
}
