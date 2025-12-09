using Google.GenAI;
using MRO.SKM.Google.Gemini.Models;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;

namespace MRO.SKM.Google.Gemini;

public class GeminiService: ILanguageModelService
{
    public Guid UUID { get; } = new Guid("D53BBE8D-DE75-4274-B742-AA3556A0B5D7");
    public string DisplayName { get; } = "Gemini 2.0 Flash";
    
    
    public async Task<string> GenerateSimpleContent(string config, string prompt)
    {
        var configModel = config.ParseAsJson<GeminiConfig>();
        
        var client = new Client(apiKey:configModel.ApiKey);
        var response = client.Models.GenerateContentStreamAsync(
            model: "gemini-2.0-flash", contents: prompt
        );

        await foreach(var item in response)
        {
            
        }

        return "halloWelt";
    }

    public LanguageModelDefaults GetDefaults()
    {
        var defaults = new LanguageModelDefaults();

        defaults.GenerateElementDocumentationSystemPrompt = Prompts.GenerateDocumentation;
        
        return defaults;
    }

    public void GetEditorConfiguration(EditorConfiguration config)
    {
        config.AddPasswordBox(e =>
        {
            e.Name = nameof(GeminiConfig.ApiKey);
            e.Lable = "Api Key";
        });
    }
}
