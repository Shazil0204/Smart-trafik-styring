using Microsoft.EntityFrameworkCore;
using Smart_traffic_controller_api.Entities;

namespace Smart_traffic_controller_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<SensorLog> SensorLogs { get; set; }

        // Overrides SaveChangesAsync to automatically log all create, update, and delete operations
        // into the AuditLogs table. Acts as a lightweight auditing trigger for tracked entities.
        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default
        )
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is not AuditLog
                    && e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted
                )
                .ToList();

            var auditLogs = new List<AuditLog>();

            foreach (var entry in entries)
            {
                var entityName = entry.Entity.GetType().Name;

                var primaryKeyProp = entry.Properties.FirstOrDefault(p =>
                    p.Metadata.IsPrimaryKey()
                );
                var entityId = primaryKeyProp?.CurrentValue?.ToString() ?? "Unknown";

                string operationType = entry.State switch
                {
                    EntityState.Added => "CREATE",
                    EntityState.Modified => "UPDATE",
                    EntityState.Deleted => "DELETE",
                    _ => "UNKNOWN",
                };

                auditLogs.Add(new AuditLog(operationType, entityName, entityId));
            }

            // Save the main entity changes first. This ensures any generated keys (e.g., IDs)
            // are available before logging the operations.
            // 'var' is used because the return type may differ based on the context.
            var result = await base.SaveChangesAsync(cancellationToken);

            // If there are audit logs to record, add them and save separately.
            if (auditLogs.Count > 0)
            {
                AuditLogs.AddRange(auditLogs);
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ;

            // Configure AuditLog entity
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.OperationType).IsRequired().HasMaxLength(50);

                entity.Property(e => e.EntityName).IsRequired().HasMaxLength(100);

                entity.Property(e => e.EntityId).IsRequired().HasMaxLength(255);
            });

            // Configure SensorLog entity
            modelBuilder.Entity<SensorLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.SensorValue).HasConversion<string>().IsRequired();
            });
        }
    }
}
