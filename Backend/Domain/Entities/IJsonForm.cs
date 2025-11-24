using System.Text.Json.Nodes;

namespace Domain.Entities;

public interface IJsonForm
{
    public Guid Guid { get; }
    public JsonNode JsonNode { get; }
    public DateTime CreatedAt { get; }
}