namespace ServiceManagerApp.Models.ViewModels.Departments
{
    public class JobRoleCheckboxViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;
    }
}
