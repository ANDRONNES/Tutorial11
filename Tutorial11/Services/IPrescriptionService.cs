using Tutorial11.DTOs;

namespace Tutorial11.Services;

public interface IPrescriptionService
{
    public Task<int> CreatePrescriptionAsync(CreatePrescriptionDTO prescription, CancellationToken ct);
}