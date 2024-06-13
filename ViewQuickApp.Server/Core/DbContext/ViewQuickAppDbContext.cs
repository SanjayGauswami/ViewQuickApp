using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;
using ViewQuickApp.Server.Core.Entities;

namespace ViewQuickApp.Server.Core.DbContext
{
    public class ViewQuickAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public ViewQuickAppDbContext(DbContextOptions<ViewQuickAppDbContext> options) : base(options)
        {
           
        }

        public DbSet<log> logs { get; set; }

        public DbSet<Message> messages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // change default table of identity 

            builder.Entity<ApplicationUser>(e =>
            {
                e.ToTable("Users");
            });


            builder.Entity<IdentityUserClaim<string>>(e =>
            {
                e.ToTable("UserClaims");
            });


            builder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable("UserLogin");
            });

            builder.Entity<IdentityUserToken<string>>(e =>
            {
                e.ToTable("UserTokens");
            });

            builder.Entity<IdentityRole>(e =>
            {
                e.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<string>>(e =>
            {
                e.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable("UserRoles");
            });
        }
    }
}
