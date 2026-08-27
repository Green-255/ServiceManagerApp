namespace ServiceManagerApp.Models.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<JobRole> JobRoles { get; set; } = [];
    }
}
