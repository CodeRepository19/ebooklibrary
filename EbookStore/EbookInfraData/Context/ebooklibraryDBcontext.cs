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

        public DbSet<Reviewes> reviewes  { get; set; }

        public DbSet<ApprovalStatus> approvalStatuses { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<Technology>().ToTable("Technology");
            modelBuilder.Entity<Reviewes>().ToTable("Reviewes");
            modelBuilder.Entity<ApprovalStatus>().ToTable("ApprovalStatus");
            modelBuilder.Entity<Event>().ToTable("Event");
        }
    }
}