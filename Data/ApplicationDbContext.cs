using Microsoft.EntityFrameworkCore;
using SubsAPI.Entities;
using SubsAPI.Models;

namespace SubsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<TokenExpiration> TokenExpiration { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }

    }
}
