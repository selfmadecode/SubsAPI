using Microsoft.EntityFrameworkCore;
using SubsAPI.Entities;
using SubsAPI.Models;
using System.Reflection;

namespace SubsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<TokenExpiration> TokenExpiration { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
