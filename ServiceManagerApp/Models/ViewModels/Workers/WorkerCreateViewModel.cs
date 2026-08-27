using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.Workers
{
    public class WorkerCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
        public Departament? Departament { get; set; }
        public JobRole? JobRole { get; set; }
        public SkillLevel? SkillLevel { get; set; }
    }
}
