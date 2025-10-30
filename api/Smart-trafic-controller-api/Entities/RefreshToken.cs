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

        private RefreshToken() { } // For EF Core

        public RefreshToken(int id, Guid userId, string tokenHash, DateTime createdAt, DateTime expiresAt)
        {
            Id = id;
            UserId = userId;
            TokenHash = tokenHash;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
        }
    }
}