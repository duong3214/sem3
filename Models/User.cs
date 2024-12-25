using Microsoft.AspNetCore.Identity;

namespace backendMuseum.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string? FullName { get; set; } // Tên đầy đủ
        public string? Gender { get; set; } // Giới tính
        public string? Interests { get; set; }
        public int? Age { get; set; } // Tuổi
        // Các thuộc tính khác
        public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
        public ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
    }
}