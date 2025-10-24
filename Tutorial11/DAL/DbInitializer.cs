using Microsoft.EntityFrameworkCore;
using Tutorial11.Models;

namespace Tutorial11.DAL;

public class DbInitializer
{
     public static async Task SeedData(ClinicDbContext context)
    {
        if (!context.Doctor.Any())
        {
            var doctors = new List<Doctor>
            {
                new() { IdDoctor = 1, FirstName = "Bob", LastName = "Bobsky", Email = "bob@bobsky.com" },
                new() { IdDoctor = 2, FirstName = "Lily", LastName = "Lionhear", Email = "lionhear@test.com" },
                new() { IdDoctor = 3, FirstName = "Anna", LastName = "Perry", Email = "test@test.com" }
            };
            context.Doctor.AddRange(doctors);
        }
        
        if (!context.Medicament.Any())
        {
            var medicaments = new List<Medicament>
            {
                new() { IdMedicament = 1, Name = "Nurofen", Description = "test", Type = "painkiller"},
                new() { IdMedicament = 2, Name = "Oceptisept", Description = "test", Type = "disinfectant"},
                new() { IdMedicament = 3, Name = "Omega-3", Description = "test", Type = "Vitamins and Minerals Complexes"}
            };
            context.Medicament.AddRange(medicaments);
        }

        
        if (!context.Patient.Any())
        {
            var patients = new List<Patient>
            {
                new() { IdPatient = 1, FirstName = "Patrick", LastName = "Sponge", BirthDate = DateTime.Now },
                new() { IdPatient = 2, FirstName = "John", LastName = "Lenon", BirthDate = DateTime.Now },
                new() { IdPatient = 3, FirstName = "Peter", LastName = "Potter", BirthDate = DateTime.Now }
            };
            context.Patient.AddRange(patients);
        }

        await context.SaveChangesAsync();

        
        if (!context.Prescription.Any())
        {
            var prescriptions = new List<Prescription>
            {
                new Prescription
                {
                    IdPrescription = 1,
                    Date = DateTime.Now,
                    DueDate = DateTime.Now.AddMonths(2),
                    IdDoctor = 1,
                    IdPatient = 1,
                    PrescriptionMedicaments = new List<PrescriptionMedicament>
                    {
                        new() { IdMedicament = 1, Dose = 1, Details = "test" }
                    }
                },
                new Prescription
                {
                    IdPrescription = 2,
                    Date = DateTime.Now,
                    DueDate = DateTime.Now.AddMonths(2),
                    IdDoctor = 2,
                    IdPatient = 2,
                    PrescriptionMedicaments = new List<PrescriptionMedicament>
                    {
                        new() { IdMedicament = 2, Dose = 1, Details = "test" }
                    }
                },
                new Prescription
                {
                    IdPrescription = 3,
                    Date = DateTime.Now,
                    DueDate = DateTime.Now.AddMonths(2),
                    IdDoctor = 3,
                    IdPatient = 3,
                    PrescriptionMedicaments = new List<PrescriptionMedicament>
                    {
                        new() { IdMedicament = 3, Dose = 1, Details = "test" }
                    }
                }
            };

            context.Prescription.AddRange(prescriptions);
            await context.SaveChangesAsync();
        }
    }
}