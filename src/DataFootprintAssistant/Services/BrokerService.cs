using DataFootprintAssistant.Data;
using DataFootprintAssistant.Models;
using Microsoft.EntityFrameworkCore;

namespace DataFootprintAssistant.Services;

public class BrokerService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public BrokerService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<BrokerRecord>> ListAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Brokers
            .AsNoTracking()
            .OrderBy(broker => broker.Name)
            .ToListAsync();
    }

    public async Task<BrokerRecord?> FindAsync(string brokerKey)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Brokers.AsNoTracking().FirstOrDefaultAsync(broker => broker.BrokerKey == brokerKey);
    }

    public async Task SaveDetailsAsync(string brokerKey, BrokerStatus status, DateTime? lastActionDate, string notes)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var row = await db.Brokers.SingleAsync(broker => broker.BrokerKey == brokerKey);
        var statusChanged = row.Status != status;
        var notesChanged = row.Notes != notes.Trim();
        var dateChanged = row.LastActionDate?.Date != lastActionDate?.Date;

        row.Status = status;
        row.LastActionDate = lastActionDate?.Date;
        row.Notes = notes.Trim();

        if (statusChanged)
        {
            AddActivity(db, row, $"Updated {row.Name} to {StatusText.Label(status)}");
        }
        else if (notesChanged || dateChanged)
        {
            AddActivity(db, row, $"Saved notes for {row.Name}");
        }

        await db.SaveChangesAsync();
    }

    public async Task MarkRequestSentAsync(string brokerKey)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var row = await db.Brokers.SingleAsync(broker => broker.BrokerKey == brokerKey);
        row.Status = BrokerStatus.RequestSent;
        row.LastActionDate = DateTime.Today;
        AddActivity(db, row, $"Marked {row.Name} as request sent");
        await db.SaveChangesAsync();
    }

    public async Task<string?> AddCustomAsync(string name, string optOutUrl)
    {
        var trimmedName = name.Trim();
        var trimmedUrl = optOutUrl.Trim();
        if (trimmedName.Length == 0)
        {
            return "Add a name for this broker.";
        }

        if (!Uri.TryCreate(trimmedUrl, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps)
        {
            return "The official page needs a full https link.";
        }

        await using var db = await _factory.CreateDbContextAsync();
        var record = new BrokerRecord
        {
            BrokerKey = "custom-" + Guid.NewGuid().ToString("N")[..8],
            Name = trimmedName,
            OptOutUrl = trimmedUrl,
            Hint = "A broker you added. Open the official page and follow their process.",
            IsCustom = true,
            Status = BrokerStatus.NotStarted,
            Notes = ""
        };
        db.Brokers.Add(record);
        AddActivity(db, record, $"Added {record.Name} to your broker list");
        await db.SaveChangesAsync();
        return null;
    }

    public static (int NotStarted, int InProgress, int Confirmed) Count(IEnumerable<BrokerRecord> brokers)
    {
        var list = brokers.ToList();
        var notStarted = list.Count(broker => broker.Status == BrokerStatus.NotStarted);
        var confirmed = list.Count(broker => broker.Status == BrokerStatus.ConfirmedDeleted);
        var inProgress = list.Count(broker =>
            broker.Status is BrokerStatus.RequestSent or BrokerStatus.NeedsFollowUp);
        return (notStarted, inProgress, confirmed);
    }

    public static BrokerRecord? NextToContinue(IEnumerable<BrokerRecord> brokers)
    {
        var list = brokers.ToList();
        return list.FirstOrDefault(broker => broker.Status == BrokerStatus.NeedsFollowUp)
            ?? list.FirstOrDefault(broker => broker.Status == BrokerStatus.RequestSent)
            ?? list.FirstOrDefault(broker => broker.Status == BrokerStatus.NotStarted);
    }

    private static void AddActivity(AppDbContext db, BrokerRecord broker, string message)
    {
        db.Activities.Add(new ActivityEntry
        {
            CreatedAt = DateTime.UtcNow,
            BrokerKey = broker.BrokerKey,
            Message = message
        });
    }
}
