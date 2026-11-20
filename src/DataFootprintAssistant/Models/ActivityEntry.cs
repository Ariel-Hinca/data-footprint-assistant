namespace DataFootprintAssistant.Models;

public class ActivityEntry
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Message { get; set; } = "";

    public string? BrokerKey { get; set; }
}
