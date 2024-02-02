using Microsoft.EntityFrameworkCore;
using Ecommerce.Notification.Entities;


namespace Ecommerce.Notification.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationEntity>()
                .HasOne<AppUser>(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany<NotificationEntity>(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<AppUser> Users { get; set; }
    }
}