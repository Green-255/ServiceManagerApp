using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        public string ReferenceCode { get; set; } = string.Empty; // unique code for each worker, maybe use a GUID or a combination of name and ID
        public string Name { get; set; } = string.Empty;
        public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.Available;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
        public int? DepartamentId { get; set; }
        public Departament? Departament { get; set; }
        public int? JobRoleId { get; set; }
        public JobRole? JobRole { get; set; }
        public SkillLevel? SkillLevel { get; set; }
        public ICollection<Service> Services { get; set; } = [];
    }
}
