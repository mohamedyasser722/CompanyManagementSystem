using CompanyManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagementSystem.DAL.Context
{
    public class CompanyManagementSystemDbContext : IdentityDbContext<ApplicationUser>
    {

        public CompanyManagementSystemDbContext(DbContextOptions<CompanyManagementSystemDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().Property(e => e.Salary).HasColumnType("decimal(18,2)");

            // ondelete set null

            modelBuilder.Entity<Employee>()
           .HasOne(e => e.Department)
           .WithMany() // No need to specify the Employees collection in the Department class
           .HasForeignKey(e => e.DepartmentId)
           .OnDelete(DeleteBehavior.SetNull); // Set null on delete

        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
