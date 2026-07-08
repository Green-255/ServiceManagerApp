using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        public string ReferenceCode { get; set; } = string.Empty; // unique code for each worker, maybe use a GUID or a combination of name and ID
        public string Name { get; set; } = string.Empty;
        public AvailabilityStatus AvailabilityStatus = AvailabilityStatus.Available;
        public int? PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public WorkSector WorkSector { get; set; } = WorkSector.Unsigned;
        public Occupation Occupation { get; set; } = Occupation.Unsigned; // prob need different enums for each WorkSector. Maybe use func?
    }
}
