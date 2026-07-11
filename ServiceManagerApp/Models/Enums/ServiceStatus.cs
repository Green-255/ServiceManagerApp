namespace ServiceManagerApp.Models.Enums
{
    public enum ServiceStatus
    {
        InProgress, // active
        Scheduled, // inactive, soon to be active
        Draft, // without details | before AI processing | before review
        NeedsReview,
        Completed,
        Cancelled
    }
}
