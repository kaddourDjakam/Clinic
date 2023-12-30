using Clinik.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Repository.DataContext
{
    public class ClinikEntities : DbContext
    {
        readonly string AppDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("Data Source = " + AppDataFolderPath + "/clinik/" + "DataBase/UserData.db");
        }
        public DbSet<Person>? Persons { get; set; }
        public DbSet<PatientModel>? Patients { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Appointment>? Appointments { get; set; }
        public DbSet<Treatment>? Treatments { get; set; }
        public DbSet<Payment>? Payments { get; set; }

        public DbSet<Prescription>? Prescriptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure your entity relationships and properties here
            // This method is where you can add configurations if needed
            // modelBuilder.Entity<YourEntity>().Property(e => e.YourProperty).IsRequired();
        }
    }

}
