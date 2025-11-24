using System.Text.Json.Nodes;

namespace WebApi.Dtos;

public class JsonFormDto
{
    public required Guid Guid { get; init; }
    public required JsonNode FormJson { get; init; }
    public required DateTime CreatedAt { get; init; }
}