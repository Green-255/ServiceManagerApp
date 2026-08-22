using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.ServiceRequests
{
    public class ServiceRequestEditViewModel
    {
        public int Id { get; init; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceRequestType ServiceRequestType { get; set; } = ServiceRequestType.NewService;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? RequestedDueUtc { get; set; }
    }
}
