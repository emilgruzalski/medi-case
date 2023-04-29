using Premedical.DataTransferObjects;

namespace Premedical.Services.Interfaces;

public interface ISynchronizationService
{
    Task<SynchronizationResultDTO> SynchronizeAsync();
}
