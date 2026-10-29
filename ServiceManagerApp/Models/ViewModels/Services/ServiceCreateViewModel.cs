using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagerApp.Models.ViewModels.Services
{
    public class ServiceCreateViewModel
    {
        public ServiceRequest ServiceRequest { get; set; } = null!;
        public ServiceRequestType ServiceRequestType { get; set; }
        public DateTime? DueAtUtc { get; set; }
        public int? DurationHours { get; set; }
        public int? DurationMinutes { get; set; }
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Service needs a thourough description.")]
        [StringLength(300, MinimumLength = 2)]
        public required string Comments { get; set; }
        public float Cost { get; set; }
        public int[] WorkersIds { get; set; } = [];

        // assigned time | role | who assigned the worker | whether the worker accepted | completion status
        //public ICollection<ServiceAssignment> Assignments { get; set; } = [];
    }
}
