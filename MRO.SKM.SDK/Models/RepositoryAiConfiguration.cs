using MRO.SMK.SDK.Models;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class RepositoryAiConfiguration: BaseEntity
{
    public Guid ProviderId { get; set; }
    public Repository Repository { get; set; }

    public bool GenerateDoc { get; set; } = true;    
    public string GenerateDocPrompt { get; set; }
    public string Configuration { get; set; }
}