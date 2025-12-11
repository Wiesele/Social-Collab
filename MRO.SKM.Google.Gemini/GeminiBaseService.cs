using Google.GenAI;
using MRO.SKM.Google.Gemini.Models;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;

namespace MRO.SKM.Google.Gemini;

public abstract class GeminiBaseService
{
    protected async Task<string> GenerateSimpleContent(string model, string apiKey, string prompt)
    {
        var client = new Client(apiKey: apiKey, vertexAI: false);
        var response = await client.Models.GenerateContentAsync(
            model: model, contents: prompt
        );

        var responseContent =  response.Candidates[0].Content.Parts[0].Text;
        
        return this.CleanupString(responseContent);
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