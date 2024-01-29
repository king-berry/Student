using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Student_Management.Models;

namespace Student_Management.Data
{
    public class Student_ManagementContext : DbContext
    {
        public Student_ManagementContext(DbContextOptions<Student_ManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Student_Management.Models.Student> Student { get; set; } = default!;
		public DbSet<Student_Management.Models.Class> Class { get; set; } = default!;
		

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Student")
                .HasIndex(p => p.Id).IsUnique();
            modelBuilder.Entity<Class>().ToTable("Class")
                .HasIndex(p => p.Name).IsUnique();
            
        }
		
    }
}
