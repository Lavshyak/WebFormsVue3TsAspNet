namespace WebApi.Dtos;

public class SearchingConfigurationDto
{
    public List<PathArrayAndValueSearchingConfigurationDto>? PathsValues { get; init; }
    public DateTime? CreatedAfterIncluding { get; init; }
    public DateTime? CreatedBefore { get; init; }
    public Guid? Guid { get; init; }
    public int? Skip { get; init; }
    public int? Take { get; init; }
}