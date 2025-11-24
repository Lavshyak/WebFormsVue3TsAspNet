using System.Text.Json.Nodes;
using DomainApp.Entities;

namespace DomainApp.Services;

public interface IStoreJsonFormService
{
    public Task<IJsonForm> Store(JsonNode jsonNode);
}