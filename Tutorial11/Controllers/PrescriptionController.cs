using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial11.DAL;
using Tutorial11.DTOs;
using Tutorial11.Models;
using Tutorial11.Services;

namespace Tutorial11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost("addNewPrescription")]
    public async Task<IActionResult> AddNewPrescription(CreatePrescriptionDTO prescriptionDto, CancellationToken ct)
    {
        var newId = await _prescriptionService.CreatePrescriptionAsync(prescriptionDto, ct);
        return Created("", new { messege = "Prescription created.", Id = newId });

    }
}