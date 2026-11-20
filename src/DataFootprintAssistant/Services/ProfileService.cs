using DataFootprintAssistant.Data;
using DataFootprintAssistant.Models;
using Microsoft.EntityFrameworkCore;

namespace DataFootprintAssistant.Services;

public class ProfileService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ProfileService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<ProfileData> GetAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        var profile = await db.Profiles
            .Include(item => item.Emails)
            .Include(item => item.Phones)
            .FirstOrDefaultAsync();

        if (profile is null)
        {
            return new ProfileData();
        }

        return new ProfileData
        {
            FullName = profile.FullName,
            Emails = profile.Emails.OrderBy(email => email.SortOrder).Select(email => email.Address).ToList(),
            Phones = profile.Phones.OrderBy(phone => phone.SortOrder).Select(phone => phone.Number).ToList()
        };
    }

    public async Task SaveAsync(string fullName, IReadOnlyList<string> emails, IReadOnlyList<string> phones)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var profile = await db.Profiles
            .Include(item => item.Emails)
            .Include(item => item.Phones)
            .FirstOrDefaultAsync();

        if (profile is null)
        {
            profile = new PersonProfile();
            db.Profiles.Add(profile);
        }

        profile.FullName = fullName.Trim();
        db.Emails.RemoveRange(profile.Emails);
        db.Phones.RemoveRange(profile.Phones);
        profile.Emails.Clear();
        profile.Phones.Clear();

        var order = 0;
        foreach (var email in emails)
        {
            profile.Emails.Add(new ContactEmail
            {
                Address = email.Trim(),
                SortOrder = order++
            });
        }

        order = 0;
        foreach (var phone in phones)
        {
            profile.Phones.Add(new ContactPhone
            {
                Number = phone.Trim(),
                SortOrder = order++
            });
        }

        db.Activities.Add(new ActivityEntry
        {
            CreatedAt = DateTime.UtcNow,
            Message = "Saved your details"
        });

        await db.SaveChangesAsync();
    }
}
