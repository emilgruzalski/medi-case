namespace Premedical.DataTransferObjects;

public class PendingChangesDTO
{
    public bool LanguagesChanged { get; set; }
    public List<PendingEntityChangeDTO> Entities { get; set; } = new();
    public bool HasChanges => LanguagesChanged || Entities.Count > 0;
}
