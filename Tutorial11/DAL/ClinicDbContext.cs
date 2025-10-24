using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tutorial11.Models;

namespace Tutorial11.DAL;

public class ClinicDbContext : DbContext
{
    public DbSet<Doctor> Doctor { get; set; }
    public DbSet<Medicament> Medicament { get; set; }
    public DbSet<Patient> Patient { get; set; }
    public DbSet<Prescription> Prescription { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicament { get; set; }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(p => p.BirthDate).HasColumnType("date");
            });
            
        modelBuilder.Entity<Prescription>(entity =>
            {
                entity.Property(p => p.Date).HasColumnType("date");
                entity.Property(p => p.DueDate).HasColumnType("date");
            });
    }
}