using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public int ServiceRequestId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceType ServiseType { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
        public DateTime DueAt { get; set; }
        public TimeSpan Duration { get; set; }
        public string Location { get; set; } = string.Empty;
        public int? WorkerId { get; set; }
        public ICollection<string> Comments { get; set; } = [];


    }
}
