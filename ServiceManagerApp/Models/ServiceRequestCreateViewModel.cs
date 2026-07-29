using Azure.Core;
using Microsoft.Identity.Client;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models
{
    public class ServiceRequestCreateViewModel
    {
        public ServiceRequestType ServiceRequestType { get; set; } = ServiceRequestType.NewService;
        public string Description { get; set; } = string.Empty;
        public DateTime RequestedDueUtc { get; set; }


        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceRequestType ServiceRequestType { get; set; } = ServiceRequestType.NewService;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Pending;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? RequestedDueUtc { get; set; }
    }
}
