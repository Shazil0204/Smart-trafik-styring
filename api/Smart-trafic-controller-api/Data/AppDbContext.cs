using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Enums;
using Microsoft.EntityFrameworkCore;

namespace Smart_trafic_controller_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<SensorLog> SensorLogs { get; set; }
        public DbSet<TrafficEvent> TrafficEvents { get; set; }

        // This acts as a trigger, so whenever savechanges is used it well be logged in AuditLog
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is not AuditLog &&
                            e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            var auditLogs = new List<AuditLog>();

            foreach (var entry in entries)
            {
                var entityName = entry.Entity.GetType().Name;

                var primaryKeyProp = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
                var entityId = primaryKeyProp?.CurrentValue?.ToString() ?? "Unknown";

                // Switch satement using lambda
                string operationType = entry.State switch
                {
                    EntityState.Added => "CREATE",
                    EntityState.Modified => "UPDATE",
                    EntityState.Deleted => "DELETE",
                    _ => "UNKNOWN"
                };

                auditLogs.Add(new AuditLog(operationType, entityName, entityId));
            }

            var result = await base.SaveChangesAsync(cancellationToken);

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

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.UserName)
                    .IsUnique();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure RefreshToken entity
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TokenHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ExpiresAt)
                    .IsRequired();

                entity.Property(e => e.IsRevoked)
                    .HasDefaultValue(false);

                entity.Property(e => e.RevokedAt)
                    .IsRequired(false);

                // Configure relationship with User
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure AuditLog entity
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Timestamp)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.OperationType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EntityId)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            // Configure SensorLog entity
            modelBuilder.Entity<SensorLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Timestamp)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.SensorType)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.SensorValue)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            // Configure TrafficEvent entity
            modelBuilder.Entity<TrafficEvent>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TimeStamp)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.VehicleDetected)
                    .HasDefaultValue(false);

                entity.Property(e => e.PedestrianDetected)
                    .HasDefaultValue(false);

                entity.Property(e => e.VehicleLightStatus)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.PedLightStatus)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.Duration)
                    .HasDefaultValue(0);
            });
        }
    }
}