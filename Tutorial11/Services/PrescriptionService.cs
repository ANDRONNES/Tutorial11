using Microsoft.EntityFrameworkCore;
using Tutorial11.DAL;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models;

namespace Tutorial11.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly ClinicDbContext _context;

    public PrescriptionService(ClinicDbContext context)
    {
        _context = context;
    }


    public async Task<int> CreatePrescriptionAsync(CreatePrescriptionDTO prescriptionDto,CancellationToken ct)
    {
        int newIdPatient;
        Doctor doctor = null;
        doctor = await _context.Doctor.FindAsync(prescriptionDto.IdDoctor,ct);
        if (doctor == null)
        {
            throw new NotFoundException("Doctor not found");
        }
        
        Patient patient = null;
        patient = await _context.Patient.FindAsync(prescriptionDto.patient.IdPatient);
        if (patient == null)
        {
            var pat = new Patient
            {
                FirstName = prescriptionDto.patient.FirstName,
                LastName = prescriptionDto.patient.LastName,
                BirthDate = prescriptionDto.patient.BirthDate,
            };
            await _context.Patient.AddAsync(pat, ct);
            await _context.SaveChangesAsync(ct);
            newIdPatient = pat.IdPatient;
        }
        else
        {
            newIdPatient = patient.IdPatient;
        }

        if (prescriptionDto.medicaments.Count >= 10)
        {
            throw new BadRequestException("The prescription can contain max 10 medicaments at the same time.");
        }
        /*foreach (var medic in prescriptionDto.medicaments)
        {
            var medicament = await _context.Medicament.FindAsync(medic.IdMedicament,ct);
            if (medicament == null)
            {
                return NotFound("Medicament not found");
            }
        }
        to jest źle bo robi dla każdego leku zapytanie do BD, co jest nieskuteczne */


        var medicamentIdsList = prescriptionDto.medicaments.Select(m => m.IdMedicament).ToList();
        //bierzemy tylko id z otrzymanych leków i tworzymy liste z nimi
        var existingMedicaments = await _context.Medicament
            .Where(m => medicamentIdsList.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync(ct);
        //tutaj bierzemy leki które są w bazie, potem bierzemy id tych leków i znowy robimy z nich listę
        var missingMedicaments = medicamentIdsList.Except(existingMedicaments).ToList();
        //odejmujemy od listy z id, listę leków które są w bazie
        if (missingMedicaments.Any())
        {
            throw new NotFoundException("Some medicaments are not found");
        }

        if (prescriptionDto.DueDate <= prescriptionDto.Date)
        {
            throw new BadRequestException("Due date cannot be earlier than date of issue");
        }

        var prescription = new Prescription
        {
            IdPatient = newIdPatient,
            IdDoctor = prescriptionDto.IdDoctor,
            Date = prescriptionDto.Date,
            DueDate = prescriptionDto.DueDate,
        };
        _context.Prescription.Add(prescription);
        await _context.SaveChangesAsync(ct);
        var newPrescriptionId = prescription.IdPrescription;


        foreach (var medId in medicamentIdsList)
        {
            var prescriptionMedicament = new PrescriptionMedicament
            {
                IdPrescription = newPrescriptionId,
                IdMedicament = medId,
                Dose = prescriptionDto.medicaments
                    .FirstOrDefault(m => m.IdMedicament == medId)?.Dose ?? 0,
                Details = prescriptionDto.medicaments
                    .FirstOrDefault(m => m.IdMedicament == medId)?.Description ?? string.Empty
            };
            _context.PrescriptionMedicament.Add(prescriptionMedicament);
        }
        await _context.SaveChangesAsync(ct);
        return newPrescriptionId;
    }
}