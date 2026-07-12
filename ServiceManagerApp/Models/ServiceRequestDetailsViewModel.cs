using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models
{
    public class ServiceRequestDetailsViewModel
    {
        public ServiceRequestType ServiceRequestType { get; set; } = ServiceRequestType.NewService;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ServiceRequestStatus RequestStatus { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime RequestedDueUtc { get; set; }
    }
}
