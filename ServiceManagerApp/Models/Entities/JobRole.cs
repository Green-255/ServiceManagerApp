namespace ServiceManagerApp.Models.Entities
{
    public class JobRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<Worker> Workers { get; set; } = [];
    }
}
