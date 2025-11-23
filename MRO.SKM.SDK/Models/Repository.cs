
using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SMK.SDK.Models;

public class Repository : NamedEntity
{
    public Repository()
    {
        this.Languages = new();
    }
    
    public string Location { get; set; }
    public Guid SourceProviderService { get; set; }
    public string SourceProviderConfiguration { get; set; }
    public List<RepositoryLanguage> Languages { get; set; }
}