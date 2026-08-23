using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.Services
{
    public class ServiceIndexListViewModel
    {
        public int Id { get; set; }
        public ServiceRequest ServiceRequest { get; set; } = null!;
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceRequestType ServiceRequestType { get; set; }
        public ServiceStatus Status { get; set; }
        public DateTime? DueAtUtc { get; set; }

    }
}
