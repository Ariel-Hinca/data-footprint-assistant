namespace DataFootprintAssistant.Models;

public enum RequestKind
{
    DeleteMyData,
    DoNotSell,
    Unsubscribe
}

public static class RequestKindText
{
    public static string Label(RequestKind kind) => kind switch
    {
        RequestKind.DeleteMyData => "Delete my data",
        RequestKind.DoNotSell => "Do not sell",
        RequestKind.Unsubscribe => "Unsubscribe",
        _ => kind.ToString()
    };

    public static string Summary(RequestKind kind) => kind switch
    {
        RequestKind.DeleteMyData => "Ask the broker to delete the personal information they hold about you.",
        RequestKind.DoNotSell => "Ask the broker not to sell or share your personal information.",
        RequestKind.Unsubscribe => "Ask the broker to stop emails, alerts, and other messages.",
        _ => ""
    };
}
