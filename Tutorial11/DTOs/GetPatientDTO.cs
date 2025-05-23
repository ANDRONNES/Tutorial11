using Tutorial11.Models;

namespace Tutorial11.DTOs;

public class GetPatientDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<GetPrescriptionDTO> Prescriptions { get; set; }
}