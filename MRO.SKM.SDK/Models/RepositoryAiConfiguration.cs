using MRO.SKM.SDK.Enums;
using MRO.SKM.SDK.Models.LanaugeModels;
using MRO.SMK.SDK.Models;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class RepositoryAiConfiguration : BaseEntity
{
    public Guid ProviderId { get; set; }
    public Repository Repository { get; set; }

    public bool GenerateDoc { get; set; } = true;
    public string GenerateDocPrompt { get; set; }
    public string Configuration { get; set; }


    // Properies für Guidline generierung
    public bool GenerateGuide { get; set; }
    public string GenerateGuidePrompt { get; set; }
    public int GenerateGuideFileCount { get; set; }
    public string GenerateGuideFileExtensions { get; set; }
    public GenerateGuideFilePickMethod  GenerateGuideFilePickMethod { get; set; }
    public int GenerateGuideThinkingBudget { get; set; }
}