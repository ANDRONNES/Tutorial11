using Tutorial11.Models;

namespace Tutorial11.DTOs;

public class CreatePrescriptionDTO
{
    public Patient patient { get; set; }
    public int IdDoctor { get; set; }
    public List<MedicamentDTO> medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}