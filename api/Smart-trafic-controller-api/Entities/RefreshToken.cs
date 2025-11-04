using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.ValueObjects;

namespace Smart_trafic_controller_api.Entities
{
    public class RefreshToken
    {
        public int Id { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public string TokenHash { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        private RefreshToken() { } // For EF Core

        public RefreshToken(Guid userId, RefreshTokenValueObject tokenHash, DateTime expiresAt)
        {
            UserId = userId;
            TokenHash = tokenHash.Hashed;
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = expiresAt;
            IsRevoked = false;
        }

        public bool IsActive => !IsRevoked && !IsExpired;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsNearExpiry(int hoursBeforeExpiry = 2)
        {
            return DateTime.UtcNow >= ExpiresAt.AddHours(-hoursBeforeExpiry);
        }

        public void Revoke()
        {
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
        }
    }
}