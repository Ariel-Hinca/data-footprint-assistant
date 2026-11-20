namespace DataFootprintAssistant.Models;

/// <summary>
/// One starter broker from brokers.json. Your status and notes live on BrokerRecord, not here.
/// </summary>
public class Broker
{
    public string Key { get; set; } = "";

    public string Name { get; set; } = "";

    public string OptOutUrl { get; set; } = "";

    public string Hint { get; set; } = "";
}
