using System.Text.Json.Nodes;

namespace DomainApp.Entities;

public class JsonForm : IJsonForm
{
    public required Guid Guid { get; init; }
    public required JsonNode JsonNode { get; init; }
    public required DateTime CreatedAt { get; init; }
}