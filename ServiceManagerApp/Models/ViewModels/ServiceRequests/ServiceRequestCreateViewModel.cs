using Azure.Core;
using Microsoft.Identity.Client;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.ServiceRequests
{
    public class ServiceRequestCreateViewModel
    {
        public ServiceRequestType ServiceRequestType { get; set; } = ServiceRequestType.NewService;
        public string Description { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime RequestedDueUtc { get; set; }
    }
}
