using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models.ViewModels.Services;

public class ServiceIndexViewModel
{
    public List<ServiceIndexListViewModel> AllServices { get; set; } = [];

    public List<ServiceIndexListViewModel> ServicesNeedingReview { get; set; } = [];
}