using System.Runtime.Serialization;
using Google.GenAI;
using Google.GenAI.Types;
using Markdig;
using MRO.SKM.Google.Gemini.Models;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using File = Google.GenAI.Types.File;


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

    public async Task<string> GenerateCodeGuide(string configString, string prompt, int thinkingBudget, IEnumerable<UploadFile> files)
    {
        var configModel = configString.ParseAsJson<GeminiConfig>();
        var fileDataList = new List<File>();
        
        var client = new Client(apiKey: configModel.ApiKey, vertexAI: false);

        
        var contents = new Content();
        contents.Role = "user";
        contents.Parts = new();
        contents.Parts.Add(new()
        {
            Text = prompt
        });
        
        foreach (var file in files)
        {
            // var fileData = await client.Files.UploadAsync(file.FileData, file.FileName);
            //
            // fileDataList.Add(fileData);
            // var fileForModel = new FileData()
            // {
            //     FileUri = fileData.Uri,
            //     DisplayName = fileData.DisplayName,
            //     MimeType = "text/plain"
            // };
            
            contents.Parts.Add(new()
            {
                InlineData = new Blob()
                {
                    Data = file.FileData,
                    MimeType = "text/plain"
                }
            });
        }
        
        var thinkConfig = new ThinkingConfig();
        thinkConfig.ThinkingBudget = thinkingBudget;
        
        var config = new GenerateContentConfig();
        config.ResponseMimeType = "text/plain";
        config.ThinkingConfig = thinkConfig;

        GenerateContentResponse reponse = null;
        try
        {
            reponse = await client.Models.GenerateContentAsync(
                model: this.ModelName,
                contents: contents,
                config: config
            );
        }
        catch (Exception e)
        {
        }

        foreach (var file in fileDataList)
        {
            await client.Files.DeleteAsync(file.Name);
        }

        if (reponse == null)
        {
            return "";
        }
        
        var generatedResponse = reponse.Candidates[0].Content.Parts[0].Text;
        
        // return Markdown.ToHtml(generatedResponse);
        return CleanupString(generatedResponse);
    }

    private string CleanupString(string str)
    {
        if (str.Contains("```json"))
        {
            str = str.Replace("```json", "").Replace("```", "");
        }
        if (str.Contains("```html"))
        {
            str = str.Replace("```html", "").Replace("```", "");
        }

        return str;
    }

    public LanguageModelDefaults GetDefaults()
    {
        var defaults = new LanguageModelDefaults();

        defaults.GenerateElementDocumentationSystemPrompt = Prompts.GenerateDocumentation;
        defaults.GenerateGuideSystemPrompt = Prompts.GenerateGuide;
        
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