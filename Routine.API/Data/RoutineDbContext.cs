using Microsoft.EntityFrameworkCore;
using Routine.API.Entities;
using System;

namespace Routine.API.Data
{
    public class RoutineDbContext : DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Company>().Property(x => x.Introduction).HasMaxLength(500);

            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);

            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Employee>().HasOne(navigationExpression: x => x.Company)
                .WithMany(navigationExpression: x => x.Employees).HasForeignKey(x => x.CompanyId)
                //.OnDelete(DeleteBehavior.Restrict);     //此处表明不是级联删除
                .OnDelete(DeleteBehavior.Cascade);     //此处表明是级联删除

            modelBuilder.Entity<Company>().HasData(new
                {
                    Id = Guid.Parse("BD8960E0D4E8421CBB6F828AAF2FB6F6"),
                    Name = "Microsoft",
                    Introduction = "Great Company"
                },
                new
                {
                    Id = Guid.Parse("2D072DAB2C514933A26EA09FE1B4F218"),
                    Name = "Google",
                    Introduction = "Don't be evil",
                });
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = Guid.Parse("2FC93C7F6959421CBF0AA9ED5D5D1625"),
                    CompanyId = Guid.Parse("BD8960E0D4E8421CBB6F828AAF2FB6F6"),
                    DateOfBirth = new DateTime(1996, 11, 4),
                    EmployeeNo = "G003",
                    FirstName = "Mary",
                    LastName = "King",
                    Gender = Gender.女
                },
                new Employee
                {
                    Id = Guid.Parse("E7A83F38EAB84FC0ABF8F1CF1C3F19AE"),
                    CompanyId = Guid.Parse("BD8960E0D4E8421CBB6F828AAF2FB6F6"),
                    DateOfBirth = new DateTime(1987, 4, 6),
                    EmployeeNo = "G097",
                    FirstName = "Kevin",
                    LastName = "Richardson",
                    Gender = Gender.男
                }
            );

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
