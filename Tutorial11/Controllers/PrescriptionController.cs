using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial11.DAL;
using Tutorial11.DTOs;
using Tutorial11.Models;

namespace Tutorial11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly ClinicDbContext _context;

    public PrescriptionController(ClinicDbContext context)
    {
        _context = context;
    }

    [HttpPost("addNewPrescription")]
    public async Task<IActionResult> AddNewPrescription(CreatePrescriptionDTO prescriptionDto, CancellationToken ct)
    {
        int NewPatientId = 0;
        Doctor doctor = null;
        doctor = await _context.Doctor.FindAsync(prescriptionDto.doctor, ct);
        if (doctor == null)
        {
            return NotFound("Doctor not found");
        }

        Patient patient = null;
        patient = await _context.Patient.FindAsync(prescriptionDto.patient.IdPatient, ct);
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
            
            NewPatientId = pat.IdPatient;
        }else
        {
            NewPatientId = patient.IdPatient;
        }

        if (prescriptionDto.medicaments.Count >= 10)
        {
            return BadRequest("The prescription can contain max 10 medicaments at the same time.");
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

        var MedicamentIdsList = prescriptionDto.medicaments.Select(m => m.IdMedicament).ToList();
        //bierzemy tylko id z otrzymanych leków i tworzymy liste z nimi
        var ExistingMedicamentsList = await _context.Medicament
            .Where(med => MedicamentIdsList.Contains(med.IdMedicament))
            .Select(med => med.IdMedicament)
            .ToListAsync();
        //tutaj bierzemy leki które są w bazie, potem bierzemy id tych leków i znowy robimy z nich listę

        var missingMedicamentsList = MedicamentIdsList.Except(ExistingMedicamentsList);
        //odejmujemy od listy z id, listę leków które są w bazie
        if (missingMedicamentsList.Any())
        {
            return NotFound("Some medicaments are not found");
        }

        if (prescriptionDto.DueDate <= prescriptionDto.Date)
        {
            return BadRequest("Due date cannot be earlier than date of issue");
        }

        var prescription = new Prescription
        {
            IdPatient = NewPatientId,
            IdDoctor = prescriptionDto.doctor,
            Date = prescriptionDto.Date,
            DueDate = prescriptionDto.DueDate,
        };
        _context.Prescription.Add(prescription);

        await _context.SaveChangesAsync(ct);
        var prescriptionId = prescription.IdPrescription;
        foreach (var medId in MedicamentIdsList)
        {
            var prescriptionMedicament = new PrescriptionMedicament
            {
                IdPrescription = prescriptionId,
                IdMedicament = medId,
                Dose = ' ',
                Details = "Lek"
            };
            _context.PrescriptionMedicament.Add(prescriptionMedicament);
        }
        await _context.SaveChangesAsync(ct);
        return Ok(prescription);
    }
}