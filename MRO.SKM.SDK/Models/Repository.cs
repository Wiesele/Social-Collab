
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SMK.SDK.Models;

public class Repository : NamedEntity
{
    public string Location { get; set; }
    public Guid SourceProviderService { get; set; }
    public string SourceProviderConfiguration { get; set; }
}