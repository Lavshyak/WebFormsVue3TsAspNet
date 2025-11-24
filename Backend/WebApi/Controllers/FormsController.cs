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
        // TODO: handle duplicated paths in searchingConfigurationDto.PathsValues
        var searchingConfiguration = new SearchingConfiguration()
        {
            Guid = searchingConfigurationDto.Guid,
            Take = searchingConfigurationDto.Take,
            Skip = searchingConfigurationDto.Skip,
            CreatedAfterIncluding = searchingConfigurationDto.CreatedAfterIncluding,
            CreatedBefore = searchingConfigurationDto.CreatedBefore,
            PathsValues = searchingConfigurationDto.PathsValues?.Select(pathAndArray => KeyValuePair.Create((ICollection<string>)pathAndArray.Path,
                (IValueSearchingConfiguration)new ValueSearchingConfiguration()
                {
                    ExactStringValue = pathAndArray.ValueSearchingConfigurationDto.ExactStringValue
                })).ToDictionary() ?? []
        };

        var forms = await jsonFormsRepository.Search(searchingConfiguration);

        var formDtos = forms.Select(f => new JsonFormDto()
        {
            CreatedAt = f.CreatedAt,
            Guid = f.Guid,
            FormJson = f.JsonNode
        });

        return formDtos;
    }
}