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

    protected ClinicDbContext()
    {
    }

    public ClinicDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<Doctor>(entity =>  Postgres nie potrzebuje nvarchar jak sqlserver, bo od razu w swoim varchar obsługuje unicode
        {
            entity.Property(p => p.FirstName).HasColumnType("nvarchar(100)");
            entity.Property(p => p.LastName).HasColumnType("nvarchar(100)");
            entity.Property(p => p.Email).HasColumnType("nvarchar(100)");
        });*/
        modelBuilder.Entity<Patient>(entity =>
            {
                // entity.Property(p => p.FirstName).HasColumnType("nvarchar(100)");
                // entity.Property(p => p.LastName).HasColumnType("nvarchar(100)");
                entity.Property(p => p.BirthDate).HasColumnType("date");
            });
            
        modelBuilder.Entity<Prescription>(entity =>
            {
                entity.Property(p => p.Date).HasColumnType("date");
                entity.Property(p => p.DueDate).HasColumnType("date");
            });
        /*modelBuilder.Entity<Medicament>(entity =>
        {
            entity.Property(p => p.Name).HasColumnType("nvarchar(100)");
            entity.Property(p => p.Description).HasColumnType("nvarchar(100)");
            entity.Property(p => p.Type).HasColumnType("nvarchar(100)");
        });*/
            
    }
}