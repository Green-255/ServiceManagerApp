namespace ServiceManagerApp.Models.ViewModels.Services.Review
{
    public class ServiceReviewWorkerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;
    }
}
