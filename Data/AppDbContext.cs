using Microsoft.EntityFrameworkCore;
using Gayrimenkul.Models;

namespace Gayrimenkul.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Property> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data - Kategoriler
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Satılık", Description = "Satılık gayrimenkuller" },
                new Category { Id = 2, Name = "Kiralık", Description = "Kiralık gayrimenkuller" },
                new Category { Id = 3, Name = "Devren", Description = "Devren satılık işyerleri" }
            );

            // Seed data - Demo kullanıcı
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Demo Kullanıcı",
                    Email = "demo@gayrimenkul.com",
                    Password = "123456", // Gerçek projede hash'lenmiş olmalı
                    Phone = "0555 123 4567",
                    CreatedAt = DateTime.Now
                }
            );

            // Seed data - Demo ilanlar
            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                    Id = 1,
                    Title = "Merkez Lokasyonda 3+1 Daire",
                    Description = "Şehir merkezinde, ulaşıma yakın, yeni yapılı 3+1 daire",
                    Price = 2500000,
                    City = "Ankara",
                    District = "Çankaya",
                    Address = "Kızılay Mahallesi",
                    SquareMeters = 120,
                    Rooms = 4,
                    Bathrooms = 2,
                    Floor = 5,
                    ImageUrl = "https://via.placeholder.com/400x300?text=Daire",
                    IsActive = true,
                    CategoryId = 1,
                    UserId = 1,
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Id = 2,
                    Title = "Bahçeli 4+1 Villa",
                    Description = "Geniş bahçeli, müstakil villa. Doğa manzaralı.",
                    Price = 5000000,
                    City = "Ankara",
                    District = "Beypazarı",
                    Address = "Çayırhan Yolu",
                    SquareMeters = 250,
                    Rooms = 5,
                    Bathrooms = 3,
                    Floor = 2,
                    ImageUrl = "https://via.placeholder.com/400x300?text=Villa",
                    IsActive = true,
                    CategoryId = 1,
                    UserId = 1,
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Id = 3,
                    Title = "Öğrenci Evi - 2+1 Kiralık",
                    Description = "Üniversiteye yakın, eşyalı kiralık daire",
                    Price = 15000,
                    City = "Ankara",
                    District = "Keçiören",
                    Address = "Aktepe Mahallesi",
                    SquareMeters = 85,
                    Rooms = 3,
                    Bathrooms = 1,
                    Floor = 3,
                    ImageUrl = "https://via.placeholder.com/400x300?text=Kiralık",
                    IsActive = true,
                    CategoryId = 2,
                    UserId = 1,
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}