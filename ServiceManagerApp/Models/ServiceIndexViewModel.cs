using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models
{
    public class ServiceIndexViewModel
    {
        public ServiceRequest ServiceRequest { get; } = null!;
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceRequestType ServiceRequestType { get; set; }
        public ServiceStatus Status { get; set; }
        public DateTime? DueAtUtc { get; set; }

    }
}
