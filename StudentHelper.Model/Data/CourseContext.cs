using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using System;

namespace StudentHelper.Model.Data
{
    public class CourseContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<SellerApplication> SellerApplications { get; set; }

        public CourseContext(DbContextOptions<CourseContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("StudentHelperDatabase")
                ?? throw new InvalidOperationException(
                    "Connection string 'StudentHelperDatabase' not found.");

            optionsBuilder.UseSqlServer(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);

            });
        }
        public CourseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>().HasKey(uc => new { uc.StudentId, uc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(uc => uc.Student)
                .WithMany(u => u.Courses)
                .HasForeignKey(uc => uc.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(uc => uc.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
