using Premedical.DataTransferObjects;

namespace Premedical.Services.Interfaces;

public interface ILearningService
{
    Task<IEnumerable<EntityLanguageGetDTO>> GetLanguagesAsync();
    Task<IEnumerable<EntityGraphObjectDTO>> GetTreeAsync(int langId);
    Task<EntityWithTranslationDTO> GetEntityAsync(Guid id, int langId);
    Task<IEnumerable<EntityDictionaryMainDTO>> GetDictionaryAsync(Guid entityId, int langId);
    Task<FileDownload> DownloadFileAsync(Guid fileId);
    Task<IEnumerable<EntitySearchResultDTO>> SearchAsync(string? query, int langId);
}
