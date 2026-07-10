using Azure.Core;
using Microsoft.Identity.Client;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models
{
    public class CreateServiceRequestViewModel
    {
        public ServiceRequestType ServiceType { get; set; } = ServiceRequestType.NewService;
        public string Description { get; set; } = string.Empty;
        public DateTime RequestedDueUtc { get; set; }

    }
}
