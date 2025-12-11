using Google.GenAI;
using MRO.SKM.Google.Gemini.Models;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;

namespace MRO.SKM.Google.Gemini;

public class Gemini2FlashService: GeminiBaseService, ILanguageModelService
{
    public Guid UUID { get; } = new Guid("D53BBE8D-DE75-4274-B742-AA3556A0B5D7");
    public string DisplayName { get; } = "Gemini 2.0 Flash";
    
    
    public async Task<string> GenerateSimpleContent(string config, string prompt, string schema)
    {
        var configModel = config.ParseAsJson<GeminiConfig>();
        var stuff = await this.GenerateSimpleContent(GeminiModels.Gemini20Flash, configModel.ApiKey, prompt, schema);

        return stuff;
    }
}
