using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.Services.Review
{
    public class ServiceRequestReviewViewModel
    {
        public int Id { get; set; }
        public int? ServiceRequestId { get; set; }
        public ServiceRequest? ServiceRequest { get; set; }
        public string ReferenceNumber { get; init; } = string.Empty;
        public ServiceRequestType ServiceRequestType { get; init; }
        public DateTime? DueAtUtc { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
        public string Location { get; set; } = string.Empty;
        public ICollection<Worker> Workers { get; set; } = [];
        public SkillLevel? SkillLevel { get; set; }
        public ICollection<string> Comments { get; set; } = [];
        public float Cost { get; set; } = 0.0f;
    }
}
