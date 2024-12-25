using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using backendMuseum.Models;
using Microsoft.AspNetCore.Identity;


namespace backendMuseum.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Các DbSet cho các bảng khác

        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<ArtworkExhibition> ArtworkExhibitions { get; set; }
        public new DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Quan trọng để Identity hoạt động

            // Đặt tên bảng cho các bảng của Identity
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            // Ánh xạ các bảng khác
            modelBuilder.Entity<UserToken>().ToTable("UserTokens");

            // Định nghĩa khóa chính cho các bảng của ứng dụng
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Artwork>().HasKey(a => a.ArtworkId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<Payment>().HasKey(p => p.PaymentId);
            modelBuilder.Entity<Exhibition>().HasKey(e => e.ExhibitionId);
            modelBuilder.Entity<ArtworkExhibition>().HasKey(ae => new { ae.ArtworkId, ae.ExhibitionId });
            modelBuilder.Entity<UserToken>().HasKey(ut => ut.TokenId);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);

            // Ánh xạ các mối quan hệ giữa các entity
            modelBuilder.Entity<UserToken>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTokens)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Artwork>()
                .HasOne(a => a.Artist)
                .WithMany(u => u.Artworks)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ArtworkExhibition>()
                .HasOne(ae => ae.Artwork)
                .WithMany()
                .HasForeignKey(ae => ae.ArtworkId);

            modelBuilder.Entity<ArtworkExhibition>()
                .HasOne(ae => ae.Exhibition)
                .WithMany()
                .HasForeignKey(ae => ae.ExhibitionId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Artwork)
                .WithMany()
                .HasForeignKey(t => t.ArtworkId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Order)
                .WithMany(o => o.Transactions)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
