using AutoMapper;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class AiContentService : IAiContentService
{
    private const string SpeechExtension = ".mp3";
    private const string SpeechContentType = "audio/mpeg";

    private readonly IEntityTranslationsRepository _translationsRepository;
    private readonly IEntityLanguageRepository _languageRepository;
    private readonly ITextTranslator _translator;
    private readonly ISpeechSynthesizer _speechSynthesizer;
    private readonly IEntityTranslationFilesRepository _filesRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;

    public AiContentService(
        IEntityTranslationsRepository translationsRepository,
        IEntityLanguageRepository languageRepository,
        ITextTranslator translator,
        ISpeechSynthesizer speechSynthesizer,
        IEntityTranslationFilesRepository filesRepository,
        IFileStorage fileStorage,
        IMapper mapper)
    {
        _translationsRepository = translationsRepository;
        _languageRepository = languageRepository;
        _translator = translator;
        _speechSynthesizer = speechSynthesizer;
        _filesRepository = filesRepository;
        _fileStorage = fileStorage;
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

    public async Task<EntityTranslationFileDTO> GenerateSpeechAsync(Guid translationId)
    {
        var translation = await GetTranslationOrThrowAsync(translationId);
        var plainContent = HtmlText.ToPlainText(translation.Content);
        var text = string.IsNullOrEmpty(plainContent) ? translation.Title : $"{translation.Title}. {plainContent}";

        await using var audio = await _speechSynthesizer.SynthesizeAsync(text, translation.Lang.LangValue);
        var stored = await _fileStorage.SaveAsync(audio, SpeechExtension, SpeechContentType);

        var file = new EntityTranslationFile
        {
            Id = Guid.NewGuid(),
            TranslationId = translation.Id,
            FileName = $"speech-{translation.Lang.LangValue}{SpeechExtension}",
            FilePath = stored.RelativePath,
            ContentType = SpeechContentType,
            Size = stored.Size,
            UploadedAt = DateTime.UtcNow
        };
        translation.Entity.ModifiedAt = file.UploadedAt;

        _filesRepository.Add(file);
        await _filesRepository.SaveChangesAsync();

        return _mapper.Map<EntityTranslationFileDTO>(file);
    }

    private async Task<EntityTranslation> GetTranslationOrThrowAsync(Guid id) =>
        await _translationsRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"Translation with id {id} was not found.");
}
