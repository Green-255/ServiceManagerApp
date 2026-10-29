using Azure.Core;
using Microsoft.Identity.Client;
using ServiceManagerApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagerApp.Models.ViewModels.ServiceRequests
{
    public class ServiceRequestCreateViewModel
    {
        public ServiceRequestType ServiceRequestType { get; set; } = ServiceRequestType.NewService;

        [Required(ErrorMessage = "Describe your request")]
        [StringLength(1000, MinimumLength = 2)]
        public string Description { get; set; } = string.Empty;

        [StringLength(70, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;
        public DateTime RequestedDueUtc { get; set; }
    }
}
