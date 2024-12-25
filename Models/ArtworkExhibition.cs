namespace backendMuseum.Models
{
    public class ArtworkExhibition
    {
        public int ArtworkId { get; set; }
        public int ExhibitionId { get; set; }
        public Artwork? Artwork { get; set; } // Có thể null
        public Exhibition? Exhibition { get; set; } // Có thể null
    }
}