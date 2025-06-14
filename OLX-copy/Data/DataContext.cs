﻿using Microsoft.EntityFrameworkCore;
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

            if (provider == "MySql")
            {
                var connStr = config.GetConnectionString("MySql");
                var connection = new MySqlConnection(connStr);
                optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
            }
            else if (provider == "SqlServer")
            {
                var connStr = config.GetConnectionString("SqlServer");
                optionsBuilder.UseSqlServer(connStr);
            }

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemImage>()
                .HasOne(ii => ii.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(ii => ii.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductGroup -> ItemImage
            modelBuilder.Entity<ItemImage>()
                .HasOne(ii => ii.ProductGroup)
                .WithMany(pg => pg.Images)
                .HasForeignKey(ii => ii.ProductGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Product>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            modelBuilder.Entity<Entities.ProductGroup>()
                .HasAlternateKey(pg => pg.Slug);

            modelBuilder.Entity<Entities.Product>()
                .HasOne(p => p.ProductGroup)
                .WithMany(pg => pg.Products)
                .HasForeignKey(p => p.GroupId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(ii => ii.Product)
                .HasForeignKey(ii => ii.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductGroup>()
                .HasMany(pg => pg.Images)
                .WithOne(ii => ii.ProductGroup)
                .HasForeignKey(ii => ii.ProductGroupId)
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

            modelBuilder.Entity<ProductGroup>().HasData(
            new ProductGroup
            {
                Id = Guid.Parse("0dc4a692-2137-4694-bcb3-684ed826b520"),
                Name = "Скло",
                Description = "Декоративні вироби, посуд з кольорового та прозорого скла",
                Slug = "glass",
                ImageUrl = "glass.jpg"
            });

            modelBuilder.Entity<ProductGroup>().HasData(
            new ProductGroup
            {
                Id = Guid.Parse("019ed4f3-1e83-406a-abbf-80f22319f7ae"),
                Name = "Електроніка",
                Description = "Сучасна електроніка для дому, роботи та розваг: смартфони, ноутбуки, телевізори, гаджети та аксесуари ",
                Slug = "electronics",
                ImageUrl = "electronics.jpg"
            });
        }
    }
}
