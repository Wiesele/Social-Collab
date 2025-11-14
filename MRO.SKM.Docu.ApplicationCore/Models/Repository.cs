using MRO.SMK.Docu.ApplicationCore.Abstracts;

namespace MRO.SMK.Docu.ApplicationCore.Models;

public class Repository : NamedEntity
{
    public string Location { get; set; }
    public Guid SourceProviderService { get; set; }
    public string SourceProviderConfiguration { get; set; }
}