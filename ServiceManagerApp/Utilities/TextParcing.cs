namespace ServiceManagerApp.Utilities
{
    public class TextParcing
    {
        public string GetDescriptionFromComments(ICollection<string> commentsToParse) // needs renaming
        {
            return string.Join("\n", commentsToParse);
        }

        public ICollection<string> ParseDescription(string description)
        {
            return description.Split();
        }
    }
}
