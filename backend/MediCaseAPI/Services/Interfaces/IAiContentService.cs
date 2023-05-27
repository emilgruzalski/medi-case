using MediCaseAPI.DataTransferObjects;

namespace MediCaseAPI.Services.Interfaces;

public interface IAiContentService
{
    Task<EntityTranslationDTO> TranslateAsync(AiTranslationPostDTO dto);
    Task<EntityTranslationFileDTO> GenerateSpeechAsync(Guid translationId);
}
