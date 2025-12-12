using Google.GenAI;
using MRO.SKM.Google.Gemini.Models;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;

namespace MRO.SKM.Google.Gemini;

public class Gemini25FlashLightService: GeminiBaseService, ILanguageModelService
{
    public override Guid UUID { get; } = new Guid("889C9EB6-F8DB-4ECE-89E7-F64298557252");
    public override string DisplayName { get; } = "Gemini 2.5 Flash Light";
    public override string ModelName { get; } = GeminiModels.Gemini25FlashLight;
}
