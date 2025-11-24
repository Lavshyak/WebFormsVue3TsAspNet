using System.Text.Json;
using DomainApp.Entities;
using DomainApp.Repositories.FormsRepository;
using Nito.AsyncEx;

namespace PersistenceRam.Repositories;

public class JsonFormsRepositoryRam : IJsonFormsRepository
{
    // can be moved to another class
    private static readonly List<IJsonForm> _list = new ();
    private static readonly AsyncReaderWriterLock _locker = new();
    
    public async Task Store(IJsonForm jsonForm)
    {
        using var wLock = await _locker.WriterLockAsync();
        _list.Add(jsonForm);
    }

    public async Task<ICollection<IJsonForm>> Search(ISearchingConfiguration searchingConfiguration)
    {
        using var rLock = await _locker.ReaderLockAsync();
        
        // I know it is not optimized

        var query = _list.Where(x =>
        {
            return searchingConfiguration.PathsValues.All(pv =>
            {
                var path = pv.Key;
                var valueSearchingConfig = pv.Value;
                // can be null
                var jsonNode = path.Aggregate(x.JsonNode, (x1, pathPart) => x1[pathPart]);
                if (jsonNode == null)
                {
                    return false;
                }

                if (valueSearchingConfig.ExactStringValue != null && jsonNode.GetValueKind() != JsonValueKind.String)
                {
                    return false;
                }
                
                if (jsonNode.GetValue<string>() == valueSearchingConfig.ExactStringValue)
                {
                    return true;
                }

                return false;
            });
        });

        if (searchingConfiguration.CreatedAfterIncluding != null)
        {
            query = query.Where(x => x.CreatedAt >= searchingConfiguration.CreatedAfterIncluding.Value);
        }

        if (searchingConfiguration.CreatedBefore != null)
        {
            query = query.Where(x => x.CreatedAt < searchingConfiguration.CreatedBefore.Value);
        }

        if (searchingConfiguration.Guid != null)
        {
            query = query.Where(x => x.Guid == searchingConfiguration.Guid);
        }

        if (searchingConfiguration.Skip != null || searchingConfiguration.Take != null)
        {
            query = query.OrderBy(x => x.Guid);
        }

        if (searchingConfiguration.Skip != null)
        {
            query = query.Skip(searchingConfiguration.Skip.Value);
        }

        if (searchingConfiguration.Take != null)
        {
            query = query.Take(searchingConfiguration.Take.Value);
        }

        return query.ToArray();
    }
}