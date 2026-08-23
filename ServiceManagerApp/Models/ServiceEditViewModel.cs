using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models
{
    public class ServiceEditViewModel
    {
        public int Id { get; set; }
        public int ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; } = null!;
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceRequestType ServiceRequestType { get; set; }
        public ServiceStatus Status { get; set; }
        public DateTime? DueAtUtc { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Location { get; set; } = string.Empty;
        public ICollection<Worker> Workers { get; set; } = [];
        public ICollection<string> Comments { get; set; } = [];
        public float Cost { get; set; } = 0.0f;
    }
}
