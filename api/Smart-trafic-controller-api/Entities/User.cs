using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string UserName { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User() { } // For EF Core

        public User(Guid id, string firstName, string lastName, string userName, string password, bool isDeleted, DateTime createdAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Password = password;
            IsDeleted = isDeleted;
            CreatedAt = createdAt;
        }
    }
}