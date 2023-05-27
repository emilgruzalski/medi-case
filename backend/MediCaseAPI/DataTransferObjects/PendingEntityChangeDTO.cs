using System.Text.Json.Serialization;

namespace MediCaseAPI.DataTransferObjects;

public class PendingEntityChangeDTO
{
    public Guid EntityId { get; set; }
    public int TypeId { get; set; }
    public string Title { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PendingChangeType ChangeType { get; set; }

    public DateTime ModifiedAt { get; set; }
}
