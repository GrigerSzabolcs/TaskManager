using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee>
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Occupation> Occupations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Setting up one-to-many relation
            builder.Entity<Occupation>()
                .HasOne(occupation => occupation.Activity)
                .WithMany(activity => activity.Occupations)
                .HasForeignKey(occupation => occupation.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Occupation>()
                .HasOne(occupation => occupation.Employee)
                .WithMany(employee => employee.Occupations)
                .HasForeignKey(occupation => occupation.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            //Creating some seeded data
            PasswordHasher<Employee> ph = new PasswordHasher<Employee>();
            Employee admin = new Employee
            {
                Id = Guid.NewGuid().ToString(),
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                FirstName = "Gipsz",
                LastName = "Jakab",
            };
            admin.PasswordHash = ph.HashPassword(admin, "Almafa123");
            builder.Entity<Employee>().HasData(admin);

            builder.Entity<IdentityRole>().HasData(
               new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
               new { Id = "2", Name = "Employee", NormalizedName = "EMPLOYEE" }
               );

            List<Activity> seededData = new List<Activity>()
            {
                new Activity(){ Title = "Meeting", Description = "" },
                new Activity(){ Title = "Programming", Description = "" },
                new Activity(){ Title = "Testing", Description = "" },
            };
            builder.Entity<Activity>().HasData(seededData);

            base.OnModelCreating(builder);
        }
    }
}
