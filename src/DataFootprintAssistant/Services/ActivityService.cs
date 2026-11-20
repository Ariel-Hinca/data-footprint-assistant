using DataFootprintAssistant.Data;
using DataFootprintAssistant.Models;
using Microsoft.EntityFrameworkCore;

namespace DataFootprintAssistant.Services;

public class ActivityService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ActivityService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task AddAsync(string message, string? brokerKey = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Activities.Add(new ActivityEntry
        {
            CreatedAt = DateTime.UtcNow,
            Message = message,
            BrokerKey = brokerKey
        });
        await db.SaveChangesAsync();
    }

    public async Task<List<ActivityEntry>> RecentAsync(int take = 5)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Activities
            .AsNoTracking()
            .OrderByDescending(entry => entry.CreatedAt)
            .Take(take)
            .ToListAsync();
    }
}
