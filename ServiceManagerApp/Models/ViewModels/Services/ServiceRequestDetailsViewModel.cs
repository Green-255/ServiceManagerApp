namespace ServiceManagerApp.Models.ViewModels.Services
{
    public class ServiceRequestDetailsViewModel
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
    }
}
