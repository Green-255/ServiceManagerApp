using Microsoft.AspNetCore.Server.HttpSys;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.Entities
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceType ServiceType { get; set; } = ServiceType.NewService;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueAt { get; set; }
    }
}
