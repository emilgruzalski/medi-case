using MediCaseAPI.DataTransferObjects;

namespace MediCaseAPI.Services.Interfaces;

public interface IEntityTranslationFileService
{
    Task<IEnumerable<EntityTranslationFileDTO>> GetByTranslationAsync(Guid translationId);
    Task<EntityTranslationFileDTO> UploadAsync(EntityTranslationFilePostDTO dto);
    Task<FileDownload> DownloadAsync(Guid id);
    Task DeleteAsync(Guid id);
}
