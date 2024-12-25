using Microsoft.AspNetCore.Identity;


namespace backendMuseum.Models
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; } // Có thể null
    }
}
