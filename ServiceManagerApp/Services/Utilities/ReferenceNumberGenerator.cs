namespace ServiceManagerApp.Services.Utilities
{
    public static class ReferenceNumberGenerator
    {
        public static string Generate(string tag, int id, string? middleTag = null)
        {
            middleTag ??= DateTime.UtcNow.Year.ToString();

            return $"{tag}-{middleTag}-{id:D6}";
        }

        public static string GenerateServiceReferenceNumber(int id)
        {
            // WO = Work Order
            return Generate("WO", id);
        }

        public static string GenerateServiceRequestReferenceNumber(int id)
        {
            // REQ = Request
            return Generate("REQ", id);
        }

        public static string GenerateWorkerReferenceNumber(int id, string departmentName)
        {
            // WIT = Worker Identity Tag
            return Generate("WIT", id, departmentName);
        }

    }
}
