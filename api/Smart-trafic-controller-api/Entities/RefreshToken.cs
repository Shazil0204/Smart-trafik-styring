using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public RefreshToken(int id, Guid userId, string tokenHash, DateTime expiresAt)
        {
            Id = id;
            UserId = userId;
            TokenHash = tokenHash;
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = expiresAt;
            IsRevoked = false;
        }

        public bool IsActive => RevokedAt == null && !IsExpired;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }
    }
}