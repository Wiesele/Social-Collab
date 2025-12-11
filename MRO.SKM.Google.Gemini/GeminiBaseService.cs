using System.Runtime.Serialization;
using Google.GenAI;
using Google.GenAI.Types;
using MRO.SKM.Google.Gemini.Models;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;

namespace MRO.SKM.Google.Gemini;

public abstract class GeminiBaseService
{
    protected async Task<string> GenerateSimpleContent(string model, string apiKey, string prompt, string schema)
    {
        var client = new Client(apiKey: apiKey, vertexAI: false);
        
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
            model: model,
            contents: contents,
            config: config
        );
        
        var responseContent = response.Candidates[0].Content.Parts[0].Text;
        
        return this.CleanupString(responseContent);
        //
        // var contents = new Content();
        // contents.Role = "user";
        // contents.Parts = new();
        // contents.Parts.Add(new()
        // {
        //     Text = prompt
        // });
        //
        // var response = client.Models.GenerateContentStreamAsync(
        //     model: model,
        //     contents: contents,
        //     config: config
        // );
        //
        // var result = "";
        //
        // await foreach (var chunk in response)
        // {
        //     result += chunk.Candidates.First().Content.Parts.First().Text;
        // }
        //
        // return result;
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