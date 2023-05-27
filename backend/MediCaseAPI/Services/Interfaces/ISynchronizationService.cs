using MediCaseAPI.DataTransferObjects;

namespace MediCaseAPI.Services.Interfaces;

public interface ISynchronizationService
{
    Task<SynchronizationResultDTO> SynchronizeAsync();
    Task<PendingChangesDTO> GetPendingChangesAsync(int langId);
}
