namespace DataFootprintAssistant.Models;

public class BrokerRecord
{
    public int Id { get; set; }

    public string BrokerKey { get; set; } = "";

    public string Name { get; set; } = "";

    public string OptOutUrl { get; set; } = "";

    public string Hint { get; set; } = "";

    public bool IsCustom { get; set; }

    public BrokerStatus Status { get; set; } = BrokerStatus.NotStarted;

    public DateTime? LastActionDate { get; set; }

    public string Notes { get; set; } = "";
}
