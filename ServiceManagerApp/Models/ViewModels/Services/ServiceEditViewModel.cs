using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using ServiceManagerApp.Models.ViewModels.Services.Review;

namespace ServiceManagerApp.Models.ViewModels.Services
{
    public class ServiceEditViewModel
    {
        public int Id { get; set; }
        public ServiceRequestSelectionDetailsViewModel? ServiceRequest { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceStatus Status { get; set; }
        public DateTime? DueAtUtc { get; set; }
        public int? DurationHours { get; set; }
        public int? DurationMinutes { get; set; }
        public string Location { get; set; } = string.Empty;
        public ServicePriority Priority { get; set; } = ServicePriority.None;
        public List<int> WorkerIds { get; set; } = [];
        public List<ServiceReviewWorkerViewModel> Workers { get; set; } = [];
        public SkillLevel? SkillLevel { get; set; }
        //[Required(ErrorMessage = "Service needs a thourough description.")]
        public string Description { get; set; } = string.Empty;
        public float Cost { get; set; } = 0.0f;
    }
}
