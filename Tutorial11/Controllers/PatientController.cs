using Microsoft.AspNetCore.Mvc;
using Tutorial11.DTOs;
using Tutorial11.Services;

namespace Tutorial11.Controllers;


[ApiController]
[Route("/api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrescription(int id, CancellationToken ct)
    {
        var client = await _patientService.GetPatientInfoAsync(id, ct);
        return Ok(client);
    }
}