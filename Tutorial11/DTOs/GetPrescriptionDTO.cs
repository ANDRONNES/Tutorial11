using Tutorial11.Models;

namespace Tutorial11.DTOs;

public class GetPrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<GetMedicamentDTO> Medicaments { get; set; }
    public Doctor doctor { get; set; }
}