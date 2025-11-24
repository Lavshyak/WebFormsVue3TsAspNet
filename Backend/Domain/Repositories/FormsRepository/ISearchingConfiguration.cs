namespace Domain.Repositories.FormsRepository;

public interface ISearchingConfiguration
{
    IDictionary<ICollection<string>, IValueSearchingConfiguration> PathsValues { get; }
    
    DateTime? CreatedAfterIncluding { get; }
    DateTime? CreatedBefore { get; }
    
    Guid? Id { get; }
    
    int? Skip { get; }
    int? Take { get; }
}