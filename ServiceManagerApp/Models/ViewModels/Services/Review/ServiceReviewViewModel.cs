using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagerApp.Models.ViewModels.Services.Review
{
    public class ServiceReviewViewModel
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; } = null!;
        public ServiceRequestDetailsViewModel? ServiceRequest { get; set; }
        public ServiceRequestType ServiceRequestType { get; init; }
        public DateTime? DueAtUtc { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;
        public ServicePriority Priority { get; set; } = ServicePriority.None;
        public List<int> WorkerIds { get; set; } = [];
        public List<ServiceReviewWorkerViewModel> Workers { get; set; } = [];
        public SkillLevel? SkillLevel { get; set; }

        //[Required(ErrorMessage = "Service needs a thourough description.")]
        //[StringLength(300, MinimumLength = 2)]
        public string Description { get; set; } = string.Empty;
        public float Cost { get; set; } = 0.0f;
    }
}
