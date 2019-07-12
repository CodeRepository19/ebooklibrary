using EbookDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace EbookInfraData.Context
{
    public class ebooklibraryDBcontext : DbContext
    {
        public ebooklibraryDBcontext(DbContextOptions<ebooklibraryDBcontext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Technology> Technologys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<Technology>().ToTable("Technology");
        }
    }
}