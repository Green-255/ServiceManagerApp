using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagerApp.Models.ViewModels.Services
{
    public class ServiceCreateViewModel
    {
        public ServiceRequest? ServiceRequest { get; set; } = null;
        public ServiceRequestType ServiceRequestType { get; set; } = ServiceRequestType.NewService;
        public DateTime? DueAtUtc { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Hours cannot be negative.")]
        public int? DurationHours { get; set; }

        [Range(0, 59, ErrorMessage = "Minutes must be between 0 and 59.")]
        public int? DurationMinutes { get; set; }

        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "Service needs a thourough description.")]
        [StringLength(300, MinimumLength = 2)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter cost.")]
        public decimal Cost { get; set; } = decimal.Zero;
        public int[] WorkersIds { get; set; } = [];

        // assigned time | role | who assigned the worker | whether the worker accepted | completion status
        //public ICollection<ServiceAssignment> Assignments { get; set; } = [];
    }
}
