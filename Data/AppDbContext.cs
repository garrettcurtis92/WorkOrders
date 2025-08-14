using Microsoft.EntityFrameworkCore;
using WorkOrders.Models;

namespace WorkOrders.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();

    // Optional: keep UpdatedAt fresh when saving
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<WorkOrder>();
        foreach (var e in entries)
        {
            if (e.State == EntityState.Added)
            {
                e.Entity.CreatedAt = DateTime.UtcNow;
                e.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (e.State == EntityState.Modified)
            {
                e.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
