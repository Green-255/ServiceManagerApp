using Microsoft.AspNetCore.Server.HttpSys;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.Entities
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceType ServiceType { get; set; } = ServiceType.NewService;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? RequestedDueUtc { get; set; }
    }
}
