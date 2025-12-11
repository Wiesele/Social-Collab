using Google.GenAI;
using MRO.SKM.Google.Gemini.Models;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;

namespace MRO.SKM.Google.Gemini;

public class Gemini25FlashService: GeminiBaseService, ILanguageModelService
{
    public Guid UUID { get; } = new Guid("D4BEFA1C-D4C4-4F99-B8ED-B0EE815F76A0");
    public string DisplayName { get; } = "Gemini 2.5 Flash";
    
    
    public async Task<string> GenerateSimpleContent(string config, string prompt)
    {
        var configModel = config.ParseAsJson<GeminiConfig>();
        var stuff = await this.GenerateSimpleContent(GeminiModels.Gemini25Flash, configModel.ApiKey, prompt);

        return stuff;
    }

 
}
