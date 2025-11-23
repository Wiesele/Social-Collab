using MRO.SMK.SDK.Models;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class RepositoryLanguage: BaseEntity
{
    public Guid ProviderId { get; set; }
    public Repository Repository { get; set; }
}