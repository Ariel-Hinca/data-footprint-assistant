using System.Text.Json;
using DataFootprintAssistant.Data;
using DataFootprintAssistant.Models;
using Microsoft.EntityFrameworkCore;

namespace DataFootprintAssistant.Services;

public class BrokerCatalog
{
    public static async Task SeedAsync(AppDbContext db, string jsonPath)
    {
        var json = await File.ReadAllTextAsync(jsonPath);
        var starters = JsonSerializer.Deserialize<List<Broker>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? [];

        var existing = await db.Brokers.ToListAsync();

        foreach (var starter in starters)
        {
            if (string.IsNullOrWhiteSpace(starter.Key) || string.IsNullOrWhiteSpace(starter.Name))
            {
                continue;
            }

            var row = existing.FirstOrDefault(broker => broker.BrokerKey == starter.Key);
            if (row is null)
            {
                db.Brokers.Add(new BrokerRecord
                {
                    BrokerKey = starter.Key.Trim(),
                    Name = starter.Name.Trim(),
                    OptOutUrl = starter.OptOutUrl.Trim(),
                    Hint = starter.Hint.Trim(),
                    IsCustom = false,
                    Status = BrokerStatus.NotStarted,
                    Notes = ""
                });
                continue;
            }

            if (row.IsCustom)
            {
                continue;
            }

            row.Name = starter.Name.Trim();
            row.OptOutUrl = starter.OptOutUrl.Trim();
            row.Hint = starter.Hint.Trim();
        }

        await db.SaveChangesAsync();
    }
}
