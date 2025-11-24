using System.Text.Json.Nodes;
using Domain.Entities;

namespace Domain.Repositories.FormsRepository;

public interface IJsonFormsRepository
{
    public Task Store(IJsonForm form);
    public Task<ICollection<IJsonForm>> Search(ISearchingConfiguration searchingConfiguration);
}