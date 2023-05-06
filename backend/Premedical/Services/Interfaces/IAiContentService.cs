using Premedical.DataTransferObjects;

namespace Premedical.Services.Interfaces;

public interface IAiContentService
{
    Task<EntityTranslationDTO> TranslateAsync(AiTranslationPostDTO dto);
}
