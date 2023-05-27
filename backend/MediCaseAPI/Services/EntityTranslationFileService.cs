using AutoMapper;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Models;
using MediCaseAPI.Repositories.Interfaces;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class EntityTranslationFileService : IEntityTranslationFileService
{
    private readonly IEntityTranslationFilesRepository _filesRepository;
    private readonly IEntityTranslationsRepository _translationsRepository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public EntityTranslationFileService(
        IEntityTranslationFilesRepository filesRepository,
        IEntityTranslationsRepository translationsRepository,
        IFileService fileService,
        IMapper mapper)
    {
        _filesRepository = filesRepository;
        _translationsRepository = translationsRepository;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EntityTranslationFileDTO>> GetByTranslationAsync(Guid translationId) =>
        _mapper.Map<IEnumerable<EntityTranslationFileDTO>>(await _filesRepository.GetByTranslationAsync(translationId));

    public async Task<EntityTranslationFileDTO> UploadAsync(EntityTranslationFilePostDTO dto)
    {
        var translation = await _translationsRepository.GetByIdAsync(dto.TranslationId)
            ?? throw new NotFoundException($"Translation with id {dto.TranslationId} was not found.");

        var stored = await _fileService.SaveAsync(dto.File);
        var file = new EntityTranslationFile
        {
            Id = Guid.NewGuid(),
            TranslationId = translation.Id,
            FileName = Path.GetFileName(dto.File.FileName),
            FilePath = stored.RelativePath,
            ContentType = dto.File.ContentType,
            Size = stored.Size,
            UploadedAt = DateTime.UtcNow
        };
        translation.Entity.ModifiedAt = file.UploadedAt;

        _filesRepository.Add(file);
        await _filesRepository.SaveChangesAsync();

        return _mapper.Map<EntityTranslationFileDTO>(file);
    }

    public async Task<FileDownload> DownloadAsync(Guid id)
    {
        var file = await GetFileOrThrowAsync(id);
        return new FileDownload(_fileService.OpenRead(file.FilePath), file.ContentType, file.FileName);
    }

    public async Task DeleteAsync(Guid id)
    {
        var file = await GetFileOrThrowAsync(id);
        file.Translation.Entity.ModifiedAt = DateTime.UtcNow;

        _filesRepository.Remove(file);
        // The physical file stays until the next synchronization, the published content may still use it.
        await _filesRepository.SaveChangesAsync();
    }

    private async Task<EntityTranslationFile> GetFileOrThrowAsync(Guid id) =>
        await _filesRepository.GetByIdAsync(id)
        ?? throw new NotFoundException($"File with id {id} was not found.");
}
