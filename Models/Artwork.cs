namespace backendMuseum.Models
{
    public class Artwork
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ArtistId { get; set; }
        public bool IsAuction { get; set; } // Đấu giá hay bán giá cố định
        public string Category { get; set; } = string.Empty; // Danh mục tác phẩm
        public string? ImageUrl { get; set; }
        public User? Artist { get; set; }
    }

}
