using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX_copy.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.UserRole> UserRoles { get; set; }
        public DbSet<Entities.UserAccess> UserAccesses { get; set; }

        public DbSet<Entities.ProductGroup> ProductGroups { get; set; }
        public DbSet<Entities.Product> Products { get; set; }
        public DbSet<Entities.ItemImage> ItemImages { get; set; }

        public DataContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // у даному методі налаштовується підключення до БД

            // дістаємось конфігурації 
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // зазначаємо рядок підключення і тип драйвера (SqlServer)
            optionsBuilder.UseSqlServer(
                config.GetConnectionString("LocalDb")
            );
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.ItemImage>()
                .HasKey(i => new { i.ItemId, i.ImageUrl });

            modelBuilder.Entity<Entities.Product>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            modelBuilder.Entity<Entities.ProductGroup>()
                .HasAlternateKey(pg => pg.Slug);

            modelBuilder.Entity<Entities.Product>()
                .HasOne(p => p.ProductGroup)
                .WithMany(pg => pg.Products)
                .HasForeignKey(p => p.GroupId);

            modelBuilder.Entity<Entities.Product>()
                .HasMany(p => p.Images)
                .WithOne()
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(i => i.ItemId);

            modelBuilder.Entity<Entities.ProductGroup>()
                .HasMany(p => p.Images)
                .WithOne()
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(i => i.ItemId);

            modelBuilder.Entity<Entities.ProductGroup>()
                .HasOne(pg => pg.ParentGroup)
                .WithMany()
                .HasForeignKey(pg => pg.ParentId);


            modelBuilder.Entity<Entities.UserAccess>()
                .HasIndex(ua => ua.Login)
                .IsUnique();

            modelBuilder.Entity<Entities.UserAccess>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAccesses) 
                .HasForeignKey(ua => ua.UserId) 
                .HasPrincipalKey(u => u.Id); 


            modelBuilder.Entity<Entities.UserAccess>()
                .HasOne(ua => ua.UserRole)
                .WithMany(ur => ur.UserAccesses)
                .HasForeignKey(ua => ua.RoleId);

            //SeedData(modelBuilder);
        }
    }
}
