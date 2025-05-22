using Tutorial11.Models;

namespace Tutorial11.DTOs;

public class CreatePrescriptionDTO
{
    public Patient patient { get; set; }
    public int doctor { get; set; }
    public List<Medicament> medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}