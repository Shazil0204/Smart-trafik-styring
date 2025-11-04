namespace Smart_traffic_controller_api.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User() { } // For EF Core

        public User(string userName, string password)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Password = password;
            IsDeleted = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            UserName = $"deleted_{Id}";
        }
    }
}
