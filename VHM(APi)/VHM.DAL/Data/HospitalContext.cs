using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.Ambulance;
using VHM.DAL.Entities.ChatEntities;
using VHM.DAL.Entities.ConsultaTask;
using VHM.DAL.Entities.DevicesEntities;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;
using VHM.DAL.Entities.ReadingEntities;

namespace VHM.DAL.Data
{
    public class HospitalContext : IdentityDbContext<ApplicationUser>
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
               .HasIndex(u => u.UserName)
               .IsUnique();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Readings> Readings { get; set; }   
        public DbSet<Report> Reports { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<ConsultaionTask> consultaionTasks { get; set; }
        public DbSet<UserWithSavedPatient> savedPatients { get; set; }
        public DbSet<SupportMessages> supportMessages { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<AmbulanceCall> AmbulanceCalls { get; set; }
        public DbSet<UserWithConnectionId> UserWithConnectionIds { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Cardiovascular> Cardiovasculars { get; set; }
        public DbSet<DiabetsFeatures> DiabetsFeatures { get; set; }
        public DbSet<BrainStroke> BrainStrokes { get; set; }
        public DbSet<KidneyFeatures> KidneyFeatures { get; set; }


 
    }
}
