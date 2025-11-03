

namespace SmartTraficControlSystem.Utilities.Models
{
    public class AccountModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}