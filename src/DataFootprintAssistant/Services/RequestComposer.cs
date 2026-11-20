using DataFootprintAssistant.Models;

namespace DataFootprintAssistant.Services;

public static class RequestComposer
{
    public static string Subject(RequestKind kind, string brokerName) => kind switch
    {
        RequestKind.DeleteMyData => $"Request to delete my personal information — {brokerName}",
        RequestKind.DoNotSell => $"Please do not sell or share my personal information — {brokerName}",
        RequestKind.Unsubscribe => $"Please unsubscribe me — {brokerName}",
        _ => $"Privacy request — {brokerName}"
    };

    public static string Compose(RequestKind kind, string brokerName, ProfileData profile)
    {
        var name = profile.HasName ? profile.FullName.Trim() : "[your full name]";
        var emailLine = profile.HasEmail ? string.Join(", ", profile.Emails) : "[your email]";
        var phoneLine = profile.HasPhone ? string.Join(", ", profile.Phones) : "[your phone number]";
        var ask = kind switch
        {
            RequestKind.DeleteMyData =>
                "I am writing to ask you to delete the personal information you hold about me, including any public listing, and to confirm when that is done.",
            RequestKind.DoNotSell =>
                "I am writing to ask you not to sell or share my personal information. Please record this as a standing opt-out and confirm when it is in place.",
            RequestKind.Unsubscribe =>
                "I am writing to unsubscribe from your emails, alerts, and marketing messages. Please remove my contact details from those lists and confirm when that is done.",
            _ => "I am writing to make a privacy request."
        };

        return $"""
            {Subject(kind, brokerName)}

            Hello,

            {ask}

            Please use these details to find my information:

            Name: {name}
            Email: {emailLine}
            Phone: {phoneLine}

            Please reply so I know this request was received.

            Thank you,
            {name}
            """;
    }
}
