namespace Premedical.DataTransferObjects;

public class SynchronizationResultDTO
{
    public DateTime PublishedAt { get; set; }
    public int Languages { get; set; }
    public int Entities { get; set; }
    public int Translations { get; set; }
    public int Files { get; set; }
    public int DictionaryEntries { get; set; }
}
