using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models
{
    public class ServiceCreateViewModel
    {
        public ServiceRequest ServiceRequest { get; set; } = null!;
        public ServiceRequestType ServiceRequestType { get; set; }
        public DateTime? DueAtUtc { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public float Cost { get; set; }
        public int[] WorkersIds { get; set; } = [];

        // assigned time | role | who assigned the worker | whether the worker accepted | completion status
        //public ICollection<ServiceAssignment> Assignments { get; set; } = [];
    }
}
