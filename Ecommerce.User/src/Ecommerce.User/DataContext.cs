using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ecommerce.User.Entities;


namespace Ecommerce.User.Database
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole<string>>().ToTable("Roles");
            modelBuilder.Entity<IdentityRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityRole<string>>().ToTable("UserClaims");
        }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}