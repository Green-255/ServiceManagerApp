using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using ServiceManagerApp.Models.ViewModels.Services.Review;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagerApp.Models.ViewModels.Services
{
    public class ServiceCreateViewModel
    {
        public int? ServiceRequestId { get; set; }
        public List<ServiceRequestSelectionDetailsViewModel> AvailableRequests { get; set; } = [];
        public DateTime? DueAtUtc { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Hours cannot be negative.")]
        public int? DurationHours { get; set; }

        [Range(0, 59, ErrorMessage = "Minutes must be between 0 and 59.")]
        public int? DurationMinutes { get; set; }
        public ServicePriority Priority { get; set; } = ServicePriority.None;
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "Service needs a thourough description.")]
        [StringLength(300, MinimumLength = 2)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter cost.")]
        public decimal Cost { get; set; } = decimal.Zero;
        public SkillLevel? SkillLevel { get; set; }
        public int[] WorkerIds { get; set; } = [];
        public List<ServiceReviewWorkerViewModel> Workers { get; set; } = [];
        // assigned time | role | who assigned the worker | whether the worker accepted | completion status
        //public ICollection<ServiceAssignment> Assignments { get; set; } = [];
    }
}
