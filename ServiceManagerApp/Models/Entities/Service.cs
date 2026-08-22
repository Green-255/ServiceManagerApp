using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public int? ServiceRequestId { get; set; }
        public ServiceRequest? ServiceRequest { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceRequestType ServiceRequestType { get; set; }
        public ServiceStatus Status { get; set; }
        public DateTime? DueAtUtc { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Location { get; set; } = string.Empty;
        public ICollection<Worker> Workers { get; set; } = [];

        // assigned time | role | who assigned the worker | whether the worker accepted | completion status
        //public ICollection<ServiceAssignment> Assignments { get; set; } = [];
        public ICollection<string> Comments { get; set; } = [];
        public float Cost { get; set; } = 0.0f;

    }
}
