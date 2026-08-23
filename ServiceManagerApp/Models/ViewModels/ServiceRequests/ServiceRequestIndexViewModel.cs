using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.ServiceRequests
{
    public class ServiceRequestIndexViewModel
    {
        public int Id { get; set; }
        public ServiceRequestType ServiceRequestType { get; set; } = ServiceRequestType.NewService;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ServiceRequestStatus Status { get; set; }
        public DateTime? RequestedDueUtc { get; set; }
        
        public string ReferenceNumber { get; set; } = string.Empty;
    }
}
