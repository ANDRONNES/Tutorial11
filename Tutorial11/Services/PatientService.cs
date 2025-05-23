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
    {/*
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
            Prescriptions = patient.Prescriptions.Select(p => new GetPrescriptionDTO
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                doctor = new Doctor
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName,
                    LastName = p.Doctor.LastName,
                    Email = p.Doctor.Email
                },
                Medicaments = p.PrescriptionMedicaments.Select(pm => new GetMedicamentDTO
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Dose = pm.Dose
                }).ToList()
            }).ToList()
        };*/
        
        var patient = await _context.Patient.Select(p => new GetPatientDTO
        {
            IdPatient = p.IdPatient,
            FirstName = p.FirstName,
            LastName = p.LastName,
            BirthDate = p.BirthDate,
            Prescriptions = p.Prescriptions
                .OrderBy(pres => pres.DueDate)
                .Select(pres => new GetPrescriptionDTO
                {
                    IdPrescription = pres.IdPrescription,
                    Date = pres.Date,
                    DueDate = pres.DueDate,
                    Medicaments = pres.PrescriptionMedicaments.Select(pm => new GetMedicamentDTO
                    {
                        IdMedicament = pm.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Medicament.Description
                    }).ToList(),
                    doctor = new Doctor()
                    {
                        IdDoctor = pres.IdDoctor,
                        FirstName = pres.Doctor.FirstName,
                        LastName = pres.Doctor.LastName,
                        Email = pres.Doctor.Email,
                    }
                }).ToList(),
        }).FirstOrDefaultAsync(p => p.IdPatient == id);
        if (patient == null)
        {
            throw new NotFoundException("Patient not found");
        }
        
        return patient;

    }
}