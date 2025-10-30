using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Smart_trafic_controller_api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<SensorLog> SensorLogs { get; set; }
        public DbSet<TrafficEvent> TrafficEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("CHAR(36)")
                    .HasConversion(
                        v => v.ToString(),
                        v => Guid.Parse(v));
                
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
                
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
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure RefreshToken entity
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                
                entity.Property(e => e.UserId)
                    .HasColumnType("CHAR(36)")
                    .HasConversion(
                        v => v.ToString(),
                        v => Guid.Parse(v));
                
                entity.Property(e => e.TokenHash)
                    .IsRequired()
                    .HasMaxLength(255);
                
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.Property(e => e.ExpiresAt)
                    .HasColumnType("DATETIME");

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
                    .HasColumnType("DATETIME")
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
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.Property(e => e.SensorType)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.SensorValue)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            // Configure TrafficEvent entity
            modelBuilder.Entity<TrafficEvent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("CHAR(36)")
                    .HasConversion(
                        v => v.ToString(),
                        v => Guid.Parse(v));
                
                entity.Property(e => e.TimeStamp)
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.Property(e => e.VehicleDetected)
                    .HasDefaultValue(false);
                
                entity.Property(e => e.PedestrianDetected)
                    .HasDefaultValue(false);
                
                entity.Property(e => e.VehicleLightStatus)
                    .HasConversion<int>();
                
                entity.Property(e => e.PedLightStatus)
                    .HasConversion<int>();
                
                entity.Property(e => e.Duration)
                    .HasDefaultValue(0);
            });
        }
    }
}