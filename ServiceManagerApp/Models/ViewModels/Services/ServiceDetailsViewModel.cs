using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.Services
{
    public class ServiceDetailsViewModel
    {
        public int Id { get; set; }
        //public int? ServiceRequestId { get; set; }
        //public ServiceRequest? ServiceRequest { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceRequestSelectionDetailsViewModel? ServiceRequest { get; set; }
        public ServiceStatus Status { get; set; }
        public DateTime? DueAtUtc { get; set; }
        public TimeSpan? Duration { get; set; }
        public ServicePriority Priority { get; set; } = ServicePriority.None;
        public string Location { get; set; } = string.Empty;
        //public ICollection<Worker> Workers { get; set; } = [];
        public string SkillLevel { get; set; } = string.Empty;

        // assigned time | role | who assigned the worker | whether the worker accepted | completion status
        //public ICollection<ServiceAssignment> Assignments { get; set; } = [];
        public string Description { get; set; } = string.Empty;
        public float Cost { get; set; } = 0.0f;
    }
}
