using DemoDapper.Dtos;
using DTO.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDapper.EntityFramwork
{
    public partial class ContextEfCore : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Test_Performance> Test_Performance { get; set; }

        public ContextEfCore(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>();
            modelBuilder.Entity<Author>().HasMany(x=>x.Books);
            modelBuilder.Entity<Book>().HasOne(x => x.Author);
            modelBuilder.Entity<Test_Performance>();

        }
    }
}
