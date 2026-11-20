namespace DataFootprintAssistant.Models;

public enum BrokerStatus
{
    NotStarted,
    RequestSent,
    ConfirmedDeleted,
    NeedsFollowUp
}

public static class StatusText
{
    public static string Label(BrokerStatus status) => status switch
    {
        BrokerStatus.NotStarted => "Not started",
        BrokerStatus.RequestSent => "Request sent",
        BrokerStatus.ConfirmedDeleted => "Confirmed deleted",
        BrokerStatus.NeedsFollowUp => "Needs follow-up",
        _ => status.ToString()
    };
}
