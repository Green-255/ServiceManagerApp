using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models
{
    public class HandleRequestViewModel
    {
        public ServiceRequest ServiceRequest { get; set; } = null!;
        public string ReferenceNumber { get;} = string.Empty;
        public ServiceRequestType ServiceRequestType { get; set; }
        public ServiceStatus Status { get; set; }
        public DateTime? DueAtUtc { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Location { get; set; } = string.Empty;
        public ICollection<Worker> Workers { get; set; } = [];
    }
}
