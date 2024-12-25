namespace backendMuseum.Models
{
    public class UserToken
    {
        public int TokenId { get; set; } // Khóa chính
        public string? UserId { get; set; } // Khóa ngoại
        public string Token { get; set; } = string.Empty; // Giá trị mặc định
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public User? User { get; set; } // Nullable
    }
}