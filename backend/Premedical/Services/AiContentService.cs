using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Exceptions;
using Premedical.Models;
using Premedical.Repositories.Interfaces;
using Premedical.Services.Interfaces;

namespace Premedical.Services;

public class AiContentService : IAiContentService
{
    private readonly IEntityTranslationsRepository _translationsRepository;
    private readonly IEntityLanguageRepository _languageRepository;
    private readonly ITextTranslator _translator;
    private readonly IMapper _mapper;

    public AiContentService(
        IEntityTranslationsRepository translationsRepository,
        IEntityLanguageRepository languageRepository,
        ITextTranslator translator,
        IMapper mapper)
    {
        _translationsRepository = translationsRepository;
        _languageRepository = languageRepository;
        _translator = translator;
        _mapper = mapper;
    }

    public async Task<EntityTranslationDTO> TranslateAsync(AiTranslationPostDTO dto)
    {
        var source = await GetTranslationOrThrowAsync(dto.SourceTranslationId);
        var targetLanguage = await _languageRepository.GetByIdAsync(dto.TargetLangId)
            ?? throw new BadRequestException($"Language with id {dto.TargetLangId} does not exist.");
        if (targetLanguage.Id == source.LangId)
            throw new BadRequestException("Source and target languages are the same.");

        var title = await _translator.TranslateTextAsync(source.Title, source.Lang.LangValue, targetLanguage.LangValue);
        var content = string.IsNullOrWhiteSpace(source.Content)
            ? source.Content
            : await _translator.TranslateHtmlAsync(source.Content, source.Lang.LangValue, targetLanguage.LangValue);

        var target = (await _translationsRepository.GetByEntityAsync(source.EntityId))
            .FirstOrDefault(t => t.LangId == targetLanguage.Id);
        if (target is null)
        {
            target = new EntityTranslation { Id = Guid.NewGuid(), EntityId = source.EntityId, LangId = targetLanguage.Id };
            _translationsRepository.Add(target);
        }

        target.Title = title.Length > FieldLengths.Title ? title[..FieldLengths.Title] : title;
        target.Content = content;
        target.ModifiedAt = DateTime.UtcNow;
        source.Entity.ModifiedAt = target.ModifiedAt;
        await _translationsRepository.SaveChangesAsync();

        return _mapper.Map<EntityTranslationDTO>(await GetTranslationOrThrowAsync(target.Id));
    }

    private async Task<EntityTranslation> GetTranslationOrThrowAsync(Guid id) =>
        await _translationsRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"Translation with id {id} was not found.");
}
