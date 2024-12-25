namespace backendMuseum.Models
{
    public class Exhibition
    {
        public int ExhibitionId { get; set; }
        public string Name { get; set; } = string.Empty; // Giá trị mặc định
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; } // Có thể null
        public string? Description { get; set; } // Có thể null
    }
}