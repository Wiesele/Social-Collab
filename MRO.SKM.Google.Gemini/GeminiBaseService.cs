using System.Runtime.Serialization;
using Google.GenAI;
using Google.GenAI.Types;
using MRO.SKM.Google.Gemini.Models;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;


namespace MRO.SKM.Google.Gemini;

public abstract class GeminiBaseService: ILanguageModelService
{
    
    public abstract Guid UUID { get; }
    public abstract string DisplayName { get; }
    public abstract string ModelName { get;  }

    public async Task<string> GenerateSimpleContent(string configString, string prompt, string schema)
    {
        var configModel = configString.ParseAsJson<GeminiConfig>();
        
        var client = new Client(apiKey: configModel.ApiKey, vertexAI: false);
        
        var contents = new Content();
        contents.Role = "user";
        contents.Parts = new();
        contents.Parts.Add(new()
        {
            Text = prompt
        });

        var thinkConfig = new ThinkingConfig();
        thinkConfig.ThinkingBudget = 0;

        var config = new GenerateContentConfig();
        config.ResponseMimeType = "application/json";
        config.ResponseSchema = Schema.FromJson(schema);
        config.ThinkingConfig = thinkConfig;

        var response = await client.Models.GenerateContentAsync(
            model: this.ModelName,
            contents: contents,
            config: config
        );
        
        var responseContent = response.Candidates[0].Content.Parts[0].Text;
        
        return this.CleanupString(responseContent);
    }

    public async Task<string> GenerateCodeGuide(string config, string prompt, int thinkingBudget, IEnumerable<byte[]> files)
    {
        await Task.Delay(5000);
        
        return "Das ist eine Dokumentation";
    }

    private string CleanupString(string str)
    {
        if (str.Contains("```json"))
        {
            str = str.Replace("```json", "").Replace("```", "");
        }

        return str;
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