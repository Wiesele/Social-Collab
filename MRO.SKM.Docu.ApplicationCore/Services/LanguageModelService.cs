using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using Microsoft.EntityFrameworkCore;
using MRO.SKM.Docu.ApplicationCore.Interfaces;
using MRO.SKM.SDK.Enums;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
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
        var test = new JsonSchemaExporterOptions()
        {
            TreatNullObliviousAsNonNullable = true,
        };
        JsonNode schema = options.GetJsonSchemaAsNode(typeof(Comment), test);
        var schemaString = schema.ToString();
        
        var prompt = configuration.GenerateDocPrompt;
        var param = new Dictionary<string, string>();
        param.Add("Code", classOrModel.Body);
        param.Add("ElementName", classOrModel.Name);
        param.Add("Format", schemaString);
        
        prompt = prompt.ReplaceVariables(param);
        
        var contentString =  await model.GenerateSimpleContent(configuration.Configuration, prompt, schemaString);
        
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
            if (repositoryAiConfiguration.GenerateGuide)
            {
                data.GenerateGuide = true;
            }
        }
        
        return data;
    }

    private IEnumerable<string> GetGuideFilePaths(string basePath, RepositoryAiConfiguration config)
    {
        var fileExtensions = config.GenerateGuideFileExtensions.Split(";");
        List<string> files = new List<string>();
        
        foreach (string file in Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories))
        {
            if (fileExtensions.Any(e => e.Trim().ToLower() == Path.GetExtension(file).ToLower()))
            {
                files.Add(file);
            }
        }

        if (config.GenerateGuideFilePickMethod == GenerateGuideFilePickMethod.Random)
        {
            var random = new Random();
            var data = files.OrderBy(x => random.Next()).Take(config.GenerateGuideFileCount);

            return data;
        }

        if (config.GenerateGuideFilePickMethod == GenerateGuideFilePickMethod.Largest)
        {
            var lstInfos = new List<FileInfo>();
            foreach (var file in files)
            {
                lstInfos.Add(new FileInfo(file));
            }

            var lstInfosOrdered = lstInfos.OrderByDescending(e => e.Length);
            return lstInfosOrdered.Take(config.GenerateGuideFileCount).Select(e => e.FullName);
        }

        return new List<string>();
    }
    
    public async Task GenerateStyleGuide(Repository repository)
    {
        var config = this.DatabaseContext.RepositoryAiConfigurations
            .Include(e => e.Repository)
            .First(e => e.GenerateGuide && e.Repository.Id == repository.Id);
        
        var service = this.LanugageModels.First(e => e.UUID == config.ProviderId);
        
        
        var files = this.GetGuideFilePaths(config.Repository.Location, config);
        
        var data = new List<byte[]>();
        foreach (var file in files)
        {
            var fileData = await File.ReadAllBytesAsync(file);
            data.Add(fileData);
        }

        var guide = await service.GenerateCodeGuide(config.Configuration, config.GenerateGuidePrompt, config.GenerateGuideThinkingBudget, data);
        
        var repo = this.DatabaseContext.Repositories.First(e => e.Id == repository.Id);
        repo.StyleGuide = guide;
        
        await this.DatabaseContext.SaveChangesAsync();
    }
}