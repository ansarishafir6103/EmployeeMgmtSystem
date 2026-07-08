using EmployeeMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API Configurations: Explicitly configuring database rules
            modelBuilder.Entity<Employee>(entity =>
            {

                // Mapping explicit table name instead of defaulting to Class plural names
                entity.ToTable("tblEmployees");
                
                // Setting explicit database constraints matching MNC specifications
                entity.HasKey(e => e.EmployeeID);
                entity.Property(e => e.EmployeeID)
                .UseIdentityColumn(1,1);

                entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(150);

                entity.Property(e => e.HashedPassword)
                .IsRequired()
                .HasMaxLength(255);


                // Enforcing unique structural indexing to protect email inputs at database level
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            });

        }
    }
}
