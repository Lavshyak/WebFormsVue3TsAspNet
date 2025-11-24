namespace DomainApp.Repositories.FormsRepository;

public class SearchingConfiguration : ISearchingConfiguration
{
    public IReadOnlyDictionary<ICollection<string>, IValueSearchingConfiguration> PathsValues { get; init; }
    public DateTime? CreatedAfterIncluding { get; init; }
    public DateTime? CreatedBefore { get; init; }
    public Guid? Guid { get; init; }
    public int? Skip { get; init; }
    public int? Take { get; init; }
}