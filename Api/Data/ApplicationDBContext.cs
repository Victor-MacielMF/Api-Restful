using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<Account>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Stock)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(191);
                entity.Property(e => e.Name).HasMaxLength(191);  // ⬅️ reduzido
                entity.Property(e => e.NormalizedName).HasMaxLength(191); // ⬅️ ESSENCIAL
                entity.Property(e => e.ConcurrencyStamp).HasColumnType("longtext");
                entity.HasIndex(e => e.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique(); // ⬅️ define o índice
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(191);
                entity.Property(e => e.ConcurrencyStamp).HasColumnType("longtext");
                entity.Property(e => e.SecurityStamp).HasColumnType("longtext");
                entity.Property(e => e.PasswordHash).HasColumnType("longtext");
                entity.Property(e => e.UserName).HasMaxLength(191);  // ⬅️ reduzido
                entity.Property(e => e.NormalizedUserName).HasMaxLength(191); // ⬅️ ESSENCIAL
                entity.Property(e => e.Email).HasMaxLength(191);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(191);
                entity.HasIndex(e => e.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique(); // ⬅️ define o índice
            });
        }
    }
}