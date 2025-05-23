using Tutorial11.DTOs;

namespace Tutorial11.Services;

public interface IPatientService
{
    public Task<GetPatientDTO> GetPatientInfoAsync(int id, CancellationToken ct);
}