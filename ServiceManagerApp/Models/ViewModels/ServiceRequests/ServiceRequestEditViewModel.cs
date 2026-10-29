using ServiceManagerApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagerApp.Models.ViewModels.ServiceRequests
{
    public class ServiceRequestEditViewModel
    {
        public int Id { get; init; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceRequestType ServiceRequestType { get; set; } = ServiceRequestType.NewService;

        [Required(ErrorMessage = "Title is required")]
        [StringLength(70, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Request must have a description")]
        [StringLength(300, MinimumLength = 2)]
        public string Description { get; set; } = string.Empty;
        public DateTime? RequestedDueUtc { get; set; }
    }
}
