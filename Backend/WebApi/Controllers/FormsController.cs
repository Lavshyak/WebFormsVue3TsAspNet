using System.Text.Json.Nodes;
using DomainApp.Repositories.FormsRepository;
using DomainApp.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FormsController : ControllerBase
{
    private readonly ILogger<FormsController> _logger;

    public FormsController(ILogger<FormsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<Guid> Store([FromBody] JsonNode jsonNode,
        [FromServices] IStoreJsonFormService storeJsonFormService)
    {
        var storedForm = await storeJsonFormService.Store(jsonNode);

        return storedForm.Guid;
    }

    [HttpPost]
    public async Task<IEnumerable<JsonFormDto>> Search([FromBody] SearchingConfigurationDto searchingConfigurationDto,
        [FromServices] IJsonFormsRepository jsonFormsRepository)
    {
        var searchingConfiguration = new SearchingConfiguration()
        {
            Id = searchingConfigurationDto.Id,
            Take = searchingConfigurationDto.Take,
            Skip = searchingConfigurationDto.Skip,
            CreatedAfterIncluding = searchingConfigurationDto.CreatedAfterIncluding,
            CreatedBefore = searchingConfigurationDto.CreatedBefore,
            PathsValues = searchingConfigurationDto.PathsValues.Select(kvp => KeyValuePair.Create(kvp.Key,
                (IValueSearchingConfiguration)new ValueSearchingConfiguration()
                {
                    Value = kvp.Value.Value
                })).ToDictionary()
        };

        var forms = await jsonFormsRepository.Search(searchingConfiguration);

        var formDtos = forms.Select(f => new JsonFormDto()
        {
            CreatedAt = f.CreatedAt,
            Guid = f.Guid,
            JsonNode = f.JsonNode
        });

        return formDtos;
    }
}