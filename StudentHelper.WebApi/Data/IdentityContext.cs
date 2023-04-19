using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.RoleEntities;

namespace StudentHelper.WebApi.Data
{
    public class IdentityContext : IdentityDbContext<User, IdentityRole, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
        {

        }
        public DbSet<AdminRole> AdminRole { get; set; }
        public DbSet<UserRole> UserRole {  get; set; }
        public DbSet<ManagerRole> ManagerRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("IdentityDbContextConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'IdentityDbContextConnection' not found.");

            optionsBuilder.UseNpgsql(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdminRole>().HasBaseType<IdentityRole>();
            modelBuilder.Entity<ManagerRole>().HasBaseType<IdentityRole>();
            modelBuilder.Entity<UserRole>().HasBaseType<IdentityRole>();
        }
    }
}
