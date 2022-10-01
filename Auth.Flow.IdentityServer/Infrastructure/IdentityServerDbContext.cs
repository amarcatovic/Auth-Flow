using Auth.Flow.IdentityServer.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Auth.Flow.IdentityServer.Infrastructure
{
    public class IdentityServerDbContext : IdentityDbContext<User, Role, long>
    {
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserClaim> UserClaim { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            /*var keysProperties = builder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x.Properties);
            foreach (var property in keysProperties)
            {
                property.ValueGenerated = ValueGenerated.OnAdd;
            }*/
        }

    }
}
