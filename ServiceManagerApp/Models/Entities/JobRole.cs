namespace ServiceManagerApp.Models.Entities
{
    public class JobRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Descriptions { get; set; }
        public int? DepartamentId { get; set; }
        public Departament? Departament { get; set; }
        public ICollection<Worker> Workers { get; set; } = [];
    }
}
