using ServiceManagerApp.Models.Enums;

namespace ServiceManagerApp.Models;

public class ServiceIndexViewModel
{
    public List<ServiceIndexListViewModel> AllServices { get; set; } = [];

    public List<ServiceIndexListViewModel> ServicesNeedingReview { get; set; } = [];
}