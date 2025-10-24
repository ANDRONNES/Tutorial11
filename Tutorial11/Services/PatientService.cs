using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tutorial11.DAL;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models;

namespace Tutorial11.Services;

public class PatientService : IPatientService
{
    private readonly IMapper _mapper;
    private readonly ClinicDbContext _context;

    public PatientService(IMapper mapper, ClinicDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<GetPatientDTO> GetPatientInfoAsync(int id, CancellationToken ct)
    {
        //WITHOUT AUTOMAPPER
        /*var patient = await _context.Patient.Select(p => new GetPatientDTO
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
        }*/
        
        //WITH AUTOMAPPER
        var patient = await _context.Patient
            .Include(p => p.Prescriptions)
                .ThenInclude(pm => pm.PrescriptionMedicaments)
                    .ThenInclude(m => m.Medicament)
            .Include(p => p.Prescriptions)
                .ThenInclude(d => d.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id, ct);
        if (patient == null) {throw new NotFoundException("Patient not found");}
        return _mapper.Map<GetPatientDTO>(patient);

    }

    public async Task<List<GetPatientDTO>> GetPatientsAsync()
    {
        var patients = await _context.Patient
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(m => m.Medicament)
            .Include(p => p.Prescriptions)
                .ThenInclude(d => d.Doctor)
            .ToListAsync();

        return _mapper.Map<List<GetPatientDTO>>(patients);
    }

    
}