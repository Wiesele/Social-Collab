
using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SMK.SDK.Models;

public class Repository : NamedEntity
{
    public Repository()
    {
        this.Languages = new();
        this.RepositoryAiConfigurations = new();
    }
    
    public string Location { get; set; }
    public Guid SourceProviderService { get; set; }
    public string SourceProviderConfiguration { get; set; }
    public List<RepositoryLanguage> Languages { get; set; }
    public List<RepositoryAiConfiguration> RepositoryAiConfigurations { get; set; }
    public string StyleGuide { get; set; }
}