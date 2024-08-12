using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QRAPI.Models;
using QRAPI.Models.LibraryAPI.Models;

namespace QRAPI.Data
{
    public class ApplicationContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Restaurant>? Restaurants { get; set; }
        public DbSet<Menu>? Menus { get; set; }
        public DbSet<Food>? Foods { get; set; }
        public DbSet<QR>? QRs { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Ticket>? Tickets { get; set; }
        public DbSet<Car>? Cars { get; set; }
        public DbSet<Location>? Locations { get; set; }
        public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
        public DbSet<Person>? Persons { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.IdNumber)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Restaurant>()
                .HasIndex(r => r.PhoneNumber)
                .IsUnique();


            modelBuilder.Entity<Food>()
                .HasOne(f => f.Category)
                .WithMany(c => c.Foods)
                .HasForeignKey(f => f.CategoryID)
                .OnDelete(DeleteBehavior.Restrict); // veya diğer uygun davranış

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Cars)
                .HasForeignKey(c => c.CategoryID)
                .OnDelete(DeleteBehavior.Restrict); // veya diğer uygun davranış

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Category)
                .WithMany(d => d.Tickets)
                .HasForeignKey(c => c.CategoryID)
                .OnDelete(DeleteBehavior.Restrict); // veya diğer uygun davranış

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Location)
                .WithMany(l => l.Tickets)
                .HasForeignKey(t => t.LocationPlace)
                .OnDelete(DeleteBehavior.Restrict); // veya diğer uygun davranış

            



        }
    }


    
}
