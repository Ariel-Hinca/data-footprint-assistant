using DataFootprintAssistant.Models;
using Microsoft.EntityFrameworkCore;

namespace DataFootprintAssistant.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PersonProfile> Profiles => Set<PersonProfile>();

    public DbSet<ContactEmail> Emails => Set<ContactEmail>();

    public DbSet<ContactPhone> Phones => Set<ContactPhone>();

    public DbSet<BrokerRecord> Brokers => Set<BrokerRecord>();

    public DbSet<ActivityEntry> Activities => Set<ActivityEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonProfile>()
            .HasMany(profile => profile.Emails)
            .WithOne()
            .HasForeignKey(email => email.PersonProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PersonProfile>()
            .HasMany(profile => profile.Phones)
            .WithOne()
            .HasForeignKey(phone => phone.PersonProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BrokerRecord>()
            .HasIndex(broker => broker.BrokerKey)
            .IsUnique();

        modelBuilder.Entity<BrokerRecord>()
            .Property(broker => broker.Status)
            .HasConversion<string>();
    }
}
