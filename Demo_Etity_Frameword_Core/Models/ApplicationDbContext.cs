using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo_Etity_Frameword_Core.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // dbcontext la 1 class trong entityframework
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
        public DbSet<Category> categories { set; get; }
        public DbSet<Product> products  { set; get; }
        // seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // seed data to table categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Điện thoại",DisplayOrder=1},
                new Category { Id = 2, Name = "Máy tính bảng", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Laptop", DisplayOrder = 3},
                new Category { Id = 4, Name = "Phụ Kiện", DisplayOrder = 4 }
                );

            //seed data to table Product
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "IPhone 14Pro", Price = 900, CategoryID = 1 },
                new Product { Id = 2, Name = "Macbook Air M1", Price = 1500, CategoryID = 3 },
                new Product { Id = 3, Name = "Apple Watch Utra 14", Price = 500, CategoryID = 4 },
                new Product { Id = 4, Name = "IPhone 13Pro", Price = 700, CategoryID = 1 },
                new Product { Id = 5, Name = "IPad Pro M1", Price = 650, CategoryID = 2 },
                new Product { Id = 6, Name = "IPhone 12Pro", Price = 600, CategoryID = 1 }
                );
        }

    }
}
