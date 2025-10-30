using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User() { } // For EF Core

        public User(Guid id, string userName, string password, bool isDeleted, DateTime createdAt)
        {
            Id = id;
            UserName = userName;
            Password = password;
            IsDeleted = isDeleted;
            CreatedAt = createdAt;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            UserName = $"deleted_{Id}";
        }
    }
}