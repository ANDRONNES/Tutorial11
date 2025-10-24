using AutoMapper;
using Tutorial11.DTOs;
using Tutorial11.Models;

namespace Tutorial11.Services;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Patient, GetPatientDTO>();
        CreateMap<Prescription, GetPrescriptionDTO>();
        CreateMap<PrescriptionMedicament, GetMedicamentDTO>();
        CreateMap<Doctor, Doctor>();

    }
}