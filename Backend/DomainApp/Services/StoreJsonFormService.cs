using System.Text.Json.Nodes;
using DomainApp.Entities;
using DomainApp.Repositories.FormsRepository;

namespace DomainApp.Services;

public class StoreJsonFormService : IStoreJsonFormService
{
    private readonly IJsonFormsRepository _repository;

    public StoreJsonFormService(IJsonFormsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IJsonForm> Store(JsonNode jsonNode)
    {
        var jsonForm = new JsonForm()
        {
            Guid = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            JsonNode = jsonNode
        };

        await _repository.Store(jsonForm);

        return jsonForm;
    }
}