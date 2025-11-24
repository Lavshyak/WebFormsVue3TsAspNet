namespace DomainApp.Repositories.FormsRepository;

public interface ISearchingConfiguration
{
    IReadOnlyDictionary<ICollection<string>, IValueSearchingConfiguration> PathsValues { get; }
    
    DateTime? CreatedAfterIncluding { get; }
    DateTime? CreatedBefore { get; }
    
    Guid? Guid { get; }
    
    int? Skip { get; }
    int? Take { get; }
}