using Microsoft.EntityFrameworkCore;
using Tutorial11.DAL;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models;

namespace Tutorial11.Services;

public class PatientService : IPatientService
{
    private readonly ClinicDbContext _context;

    public PatientService(ClinicDbContext context)
    {
        _context = context;
    }
    public async Task<GetPatientDTO> GetPatientInfoAsync(int id, CancellationToken ct)
    {
        var patient = await _context.Patient
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id, ct);
        if (patient == null)
        {
            throw new NotFoundException("Patient not found");
        }
        
        var pat = new GetPatientDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions.Select(pres => new GetPrescriptionDTO
            {
                IdPrescription = pres.IdPrescription,
                Date = pres.Date,
                DueDate = pres.DueDate,
                doctor = new Doctor
                {
                    IdDoctor = pres.Doctor.IdDoctor,
                    FirstName = pres.Doctor.FirstName,
                    LastName = pres.Doctor.LastName,
                    Email = pres.Doctor.Email
                },
                Medicaments = pres.PrescriptionMedicaments.Select(pm => new GetMedicamentDTO
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Dose = pm.Dose
                }).ToList()
            }).ToList()
        };
        
        
        return pat;

    }
}