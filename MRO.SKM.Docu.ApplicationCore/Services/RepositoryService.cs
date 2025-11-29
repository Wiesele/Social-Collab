using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.Docu.ApplicationCore.Interfaces;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.Docu.ApplicationCore.Extensions;
using MRO.SMK.SDK.Models;

namespace MRO.SMK.Docu.ApplicationCore.Services;

public class RepositoryService
{
    private IDatabaseContext Database { get; set; }
    private SettingService SettingService { get; set; }
    private IServiceProvider Services { get; set; }
    private IEnumerable<ILanguageProviderService> LanguagesProviderServices { get; set; }
    private SourceControlService SourceControlService { get; set; }


    public RepositoryService(IDatabaseContext database,
        SettingService settingService,
        IServiceProvider services,
        IEnumerable<ILanguageProviderService> languagesProviderServices,
        SourceControlService sourceControlService)
    {
        this.Database = database;
        this.SettingService = settingService;
        this.Services = services;
        this.LanguagesProviderServices = languagesProviderServices;
        this.SourceControlService = sourceControlService;
    }

    public async Task<Repository> CreateAndClone(string repoName, Guid sourceProvider,
        Dictionary<string, object> sourceProviderConfig)
    {
        var selectedSourceProvider = this.SourceControlService.GetSourceProvider(sourceProvider);

        var settings = this.SettingService.GetSettings();

        var repository = new Repository();
        repository.Name = repoName;
        repository.SourceProviderService = selectedSourceProvider.UUID;
        repository.SourceProviderConfiguration = sourceProviderConfig.AsJson();
        repository.Location = Path.Join(settings.Git.LocalFolder, repository.Name + "_" + Guid.NewGuid());

        this.Database.Repositories.Add(repository);
        await this.Database.SaveChangesAsync();


    await this.SourceControlService.CloneRepository(repository);

        return repository;
    }

    public Repository GetById(string id)
    {
        return this.GetById(Guid.Parse(id));
    }

    public Repository GetById(Guid id)
    {
        return this.Database.Repositories
            .Include(r => r.Languages)
            .First(e => e.Id == id);
    }

    public List<Repository> List()
    {
        return this.Database.Repositories.ToList();
    }

    public void AddLanguage(Repository repository, Guid providerGuid)
    {
        var repoLangugae = new RepositoryLanguage()
        {
            ProviderId = providerGuid,
        };

        var repo = this.GetById(repository.Id);
        repo.Languages.Add(repoLangugae);

        this.Database.SaveChanges();
    }

    public List<RepositoryLanguage> ListLanguages(Repository repository)
    {
        return this.Database.RepositoryLanguages.Include(e => e.Repository).Where(e => e.Repository.Id == repository.Id)
            .ToList();
    }

    public async Task ReloadRepository(Repository repository)
    {
        foreach (var language in repository.Languages)
        {
            var provider = this.LanguagesProviderServices.GetLanguageProvider(language);

            var models = await provider.AnalyzeRepository(repository);


            await using var transaction = await this.Database.BeginTransactionAsync();
            await this.SyncCodeFilesToDatabase(repository, models);

            await transaction.CommitAsync();
        }
    }


    private async Task SyncCodeFilesToDatabase(Repository repository, List<CodeFile> models)
    {
        var repo = this.GetById(repository.Id);

        foreach (var codeFile in models)
        {
            var inDb = this.Database.CodeFiles
                .Include(e => e.Repository)
                .FirstOrDefault(e => e.Repository.Id == repo.Id && e.Key == codeFile.Key);

            if (inDb == null)
            {
                codeFile.Repository = repo;
                await this.Database.CodeFiles.AddAsync(codeFile);
                
                inDb = codeFile;
            }
            else
            {
                inDb.Name = codeFile.Name;
                inDb.Key = codeFile.Key;
                inDb.Body = codeFile.Body;
            }

            await this.Database.SaveChangesAsync();
            await this.SyncClassesToDatabase(inDb, codeFile.Classes);
        }

        var existing = await this.Database.CodeFiles
            .Include(e => e.Repository)
            .Where(e => e.Repository.Id == repo.Id).ToListAsync();
        var toDelete = existing.Where(p => models.All(p2 => p2.Key != p.Key)).ToList();

        await this.DeleteCodeFiles(toDelete);

        await this.Database.SaveChangesAsync();
    }

    private async Task SyncClassesToDatabase(CodeFile file, List<Class> models)
    {
        var repo = this.Database.CodeFiles.First(e => e.Id == file.Id);

        foreach (var classModel in models)
        {
            var inDb = this.Database.Classes
                .Include(e => e.CodeFile)
                .FirstOrDefault(e => e.CodeFile.Id == repo.Id && e.Key == classModel.Key);

            if (inDb == null)
            {
                classModel.CodeFile = repo;
                await this.Database.Classes.AddAsync(classModel);
                
                inDb = classModel;
            }
            else
            {
                inDb.Name = classModel.Name;
                inDb.Key = classModel.Key;
                inDb.Body = classModel.Body;
                inDb.Comment = classModel.Comment;
            }
            
            await this.Database.SaveChangesAsync();
            await this.SyncMethodsToDatabase(inDb, classModel.Methods);
        }

        var existing = await this.Database.Classes
            .Include(e => e.CodeFile)
            .Where(e => e.CodeFile.Id == file.Id).ToListAsync();

        var toDelete = existing.Where(p => models.All(p2 => p2.Key != p.Key)).ToList();

        await this.DeleteClasses(toDelete);

        await this.Database.SaveChangesAsync();
    }

    private async Task SyncMethodsToDatabase(Class cClass, List<Method> models)
    {
        var classDbModel = this.Database.Classes.First(e => e.Id == cClass.Id);

        foreach (var method in models)
        {
            var inDb = this.Database.Methods
                .Include(e => e.Class)
                .FirstOrDefault(e => e.Class.Id == classDbModel.Id && e.Key == method.Key);

            if (inDb == null)
            {
                method.Class = classDbModel;
                await this.Database.Methods.AddAsync(method);
            }
            else
            {
                inDb.Name = method.Name;
                inDb.Key = method.Key;
                inDb.Body = method.Body;
                inDb.Comment = method.Comment;
            }
            
            await this.Database.SaveChangesAsync();
        }

        var existing = await this.Database.Methods
            .Include(e => e.Class)
            .Where(e => e.Class.Id == classDbModel.Id).ToListAsync();
        var toDelete = existing.Where(p => models.All(p2 => p2.Key != p.Key)).ToList();
        await this.DeleteMethods(toDelete);

        await this.Database.SaveChangesAsync();
    }


    private async Task DeleteCodeFiles(List<CodeFile> models)
    {
        var toDelete = new List<CodeFile>();

        foreach (var item in models)
        {
            var say = await this.Database.CodeFiles
                .Include(e => e.Classes)
                .Where(e => e.Id == item.Id).FirstAsync();
            
            await DeleteClasses(say.Classes);
            
            toDelete.Add(say);
        }
        
        this.Database.CodeFiles.RemoveRange(toDelete);
        await this.Database.SaveChangesAsync();
    }

    private async Task DeleteClasses(List<Class> models)
    {
        var toDelete = new List<Class>();

        foreach (var item in models)
        {
            var say = await this.Database.Classes
                .Include(e => e.Methods)
                .Where(e => e.Id == item.Id).FirstAsync();
            
            await DeleteMethods(say.Methods);
            
            toDelete.Add(say);
        }
        
        this.Database.Classes.RemoveRange(toDelete);
        await this.Database.SaveChangesAsync();
    }

    private async Task DeleteMethods(List<Method> models)
    {
        var toDelete = new List<Method>();
        foreach (var item in models)
        {
            var say = await this.Database.Methods
                .Where(e => e.Id == item.Id).FirstAsync();
            
            toDelete.Add(say);
        }
        
        this.Database.Methods.RemoveRange(toDelete);
        await this.Database.SaveChangesAsync();
    }
}