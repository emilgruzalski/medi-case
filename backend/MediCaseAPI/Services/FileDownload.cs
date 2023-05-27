namespace MediCaseAPI.Services;

public record FileDownload(Stream Content, string ContentType, string FileName);
