using Google.GenAI;
using MRO.SKM.Google.Gemini.Models;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;

namespace MRO.SKM.Google.Gemini;

public class Gemini25FlashService: GeminiBaseService
{
    public override Guid UUID { get; } = new Guid("D4BEFA1C-D4C4-4F99-B8ED-B0EE815F76A0");
    public override string DisplayName { get; } = "Gemini 2.5 Flash";
    public override string ModelName { get; } = GeminiModels.Gemini25Flash;
}
