using Microsoft.EntityFrameworkCore;

namespace Patients.APP.Domain;

public class PatientsDb : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<DoctorPatient> DoctorPatients { get; set; }
    
    public PatientsDb(DbContextOptions<PatientsDb> options) : base(options)
    {
            
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Branch>().HasIndex(branchEntity => branchEntity.Title).IsUnique();
        
        modelBuilder.Entity<Branch>()
            .HasMany(branchEntity => branchEntity.Doctors)
            .WithOne(doctorEntity => doctorEntity.Branch)
            .HasForeignKey(doctorEntity => doctorEntity.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<DoctorPatient>().HasKey(doctorPatientEntity => new { doctorPatientEntity.DoctorId, doctorPatientEntity.PatientId });
        
        modelBuilder.Entity<DoctorPatient>()
            .HasOne(doctorPatientEntity => doctorPatientEntity.Doctor)
            .WithMany(doctorEntity => doctorEntity.DoctorPatients)
            .HasForeignKey(doctorPatientEntity => doctorPatientEntity.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<DoctorPatient>()
            .HasOne(doctorPatientEntity => doctorPatientEntity.Patient)
            .WithMany(patientEntity => patientEntity.DoctorPatients)
            .HasForeignKey(doctorPatientEntity => doctorPatientEntity.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }

        
}