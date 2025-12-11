using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using Microsoft.EntityFrameworkCore;
using MRO.SKM.Docu.ApplicationCore.Interfaces;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models.Comments;
using MRO.SKM.SDK.Models.LanaugeModels;
using MRO.SMK.SDK.Models;

namespace MRO.SMK.Docu.ApplicationCore.Services;

public class LanguageModelService
{
    private IDatabaseContext DatabaseContext { get; set; }
    private IEnumerable<ILanguageModelService> LanugageModels { get; set; }
    
    public LanguageModelService(IEnumerable<ILanguageModelService> languageModels,
        IDatabaseContext databaseContext) 
    {
        this.DatabaseContext = databaseContext;
        this.LanugageModels = languageModels;
    }

    public async Task<Comment> GenerateDocumentation(Repository repository, ICommentable classOrModel)
    {
    
        var configuration = this.DatabaseContext
            .RepositoryAiConfigurations
            .Include(e => e.Repository)
            .FirstOrDefault(e => e.GenerateDoc == true && e.Repository.Id == repository.Id);

        if (configuration == null)
        {
            return null;
        }
        
        var model = this.LanugageModels.First(e => e.UUID == configuration.ProviderId);

        
        JsonSerializerOptions options = JsonSerializerOptions.Default;
        JsonNode schema = options.GetJsonSchemaAsNode(typeof(Comment));
        
        var prompt = configuration.GenerateDocPrompt;
        var param = new Dictionary<string, string>();
        param.Add("Code", classOrModel.Body);
        param.Add("ElementName", classOrModel.Name);
        param.Add("Format", schema["properties"].ToString());
        
        prompt = prompt.ReplaceVariables(param);
        
        var contentString =  await model.GenerateSimpleContent(configuration.Configuration, prompt, schema.ToString());
        
        return contentString.ParseAsJson<Comment>();
    }

    public LmRepositoryFeatures GetRepositoryFeatures(Repository repository)
    {
        var config = this.DatabaseContext
            .Repositories
            .Include(e => e.RepositoryAiConfigurations)
            .FirstOrDefault(e => e.Id == repository.Id);
        var data = new LmRepositoryFeatures();

        if (config == null)
        {
            return data;
        }


        foreach (var repositoryAiConfiguration in config.RepositoryAiConfigurations)
        {
            if (repositoryAiConfiguration.GenerateDoc)
            {
                data.GenerateDoc = true;
            }
        }
        
        return data;
    }
}