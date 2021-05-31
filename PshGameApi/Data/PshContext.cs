using Microsoft.EntityFrameworkCore;
using PshGameApi.Models;

namespace PshGameApi.Data
{
    public class PshContext : DbContext
    {
        public PshContext(DbContextOptions<PshContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Stat> Stats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Stat>().ToTable("Stats");
        }

    }
}