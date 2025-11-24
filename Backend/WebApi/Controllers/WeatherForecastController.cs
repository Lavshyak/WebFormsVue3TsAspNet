using System.Text.Json.Nodes;
using Domain.Repositories.FormsRepository;
using Microsoft.AspNetCore.Mvc;
using PersistenceRam.Repositories;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task Store([FromBody] JsonNode jsonNode, [FromServices] IJsonFormsRepository repository)
    {
        repository.Store();
    }

    public record ValueToSearch(string Value, bool Contains);
    
    [HttpGet]
    public async Task Search(Dictionary<string, ValueToSearch> searchingConfig)
    {
        
    }
}
