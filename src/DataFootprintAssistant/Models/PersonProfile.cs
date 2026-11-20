namespace DataFootprintAssistant.Models;

public class PersonProfile
{
    public int Id { get; set; }

    public string FullName { get; set; } = "";

    public List<ContactEmail> Emails { get; set; } = [];

    public List<ContactPhone> Phones { get; set; } = [];
}

public class ContactEmail
{
    public int Id { get; set; }

    public int PersonProfileId { get; set; }

    public string Address { get; set; } = "";

    public int SortOrder { get; set; }
}

public class ContactPhone
{
    public int Id { get; set; }

    public int PersonProfileId { get; set; }

    public string Number { get; set; } = "";

    public int SortOrder { get; set; }
}

/// <summary>
/// The name, emails, and phones a page shows. The database stores these as PersonProfile rows.
/// </summary>
public class ProfileData
{
    public string FullName { get; set; } = "";

    public List<string> Emails { get; set; } = [];

    public List<string> Phones { get; set; } = [];

    public bool HasName => !string.IsNullOrWhiteSpace(FullName);

    public bool HasEmail => Emails.Count > 0;

    public bool HasPhone => Phones.Count > 0;
}
