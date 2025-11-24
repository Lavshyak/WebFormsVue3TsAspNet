namespace WebApi.Dtos;

public class SearchingConfigurationDto
{
    public IDictionary<ICollection<string>, ValueSearchingConfigurationDto> PathsValues { get; init; }
    public DateTime? CreatedAfterIncluding { get; init; }
    public DateTime? CreatedBefore { get; init; }
    public Guid? Id { get; init; }
    public int? Skip { get; init; }
    public int? Take { get; init; }
}