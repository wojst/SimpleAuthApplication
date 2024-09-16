using Microsoft.EntityFrameworkCore;
using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Auth> Auths { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Auth)
                .WithOne(a => a.User)
                .HasForeignKey<Auth>(a => a.UserId);

            modelBuilder.Entity<Auth>()
                .HasMany(a => a.Tokens)
                .WithOne(t => t.Auth) 
                .HasForeignKey(t => t.AuthId);

            modelBuilder.Entity<CurrencyRate>()
                .Property(c => c.Mid)
                .HasColumnType("decimal(18, 4)");
        }
    }
}
