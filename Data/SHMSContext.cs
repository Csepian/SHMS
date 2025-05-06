using Microsoft.EntityFrameworkCore;
using SHMS.Model;

namespace SHMS.Data
{
    public class SHMSContext : DbContext
    {
        public SHMSContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between Hotel and Room
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Hotel and Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Reviews)
                .HasForeignKey(r => r.HotelID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between User and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(c =>c.Bookings)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Payment and User
            modelBuilder.Entity<Payment>()
                .HasOne(b => b.User)
                .WithMany(c => c.Payments)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Restrict);


            // Configure one-to-one relationship between Payment and Booking
            modelBuilder.Entity<Payment>()
                .HasOne(b => b.Booking)
                .WithOne(p => p.Payment)
                .HasForeignKey<Payment>(b => b.BookingID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-one relationship between User(Manager) and Hotel
            modelBuilder.Entity<Hotel>()
                .HasOne(b => b.Manager)
                .WithOne(p => p.Hotel)
                .HasForeignKey<Hotel>(b => b.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
        .HasQueryFilter(u => u.Role == "manager");


        }
    
}
}
