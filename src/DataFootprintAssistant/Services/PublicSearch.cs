namespace DataFootprintAssistant.Services;

public static class PublicSearch
{
    public static string Google(IEnumerable<string> values)
    {
        var parts = values
            .Select(value => value.Trim())
            .Where(value => value.Length > 0)
            .Select(value => "\"" + value + "\"");
        return "https://www.google.com/search?q=" + Uri.EscapeDataString(string.Join(" OR ", parts));
    }
}
