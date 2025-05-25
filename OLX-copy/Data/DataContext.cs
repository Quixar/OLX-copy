using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using OLX_copy.Data.Entities;
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

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string provider = config["DatabaseProvider"];

                config.GetConnectionString("LocalDb")

                config.GetConnectionString("LocalDb")

            );
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemImage>()
                .HasIndex(i => new { i.ItemId, i.ImageUrl })
                .IsUnique();

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
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Cascade); // пусть удаляется с Product

            modelBuilder.Entity<Entities.ProductGroup>()
                .HasMany(p => p.Images)
                .WithOne()
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

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
                
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole 
                {   Id = "user", 
                    Description = "Self registred user",
                    CanCreate = true,
                    CanRead = true,
                    CanUpdate = true,
                    CanDelete = true
                }
            );
        }
    }
}
