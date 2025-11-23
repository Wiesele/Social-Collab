using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.Docu.ApplicationCore.Interfaces;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SMK.Docu.ApplicationCore.Services;

public class RepositoryService
{
    private IDatabaseContext Database { get; set; }
    private SettingService SettingService { get; set; }
    private IServiceProvider Services { get; set; }


    public RepositoryService(IDatabaseContext database,
        SettingService settingService,
        IServiceProvider services)
    {
        this.Database = database;
        this.SettingService = settingService;
        this.Services = services;
    }

    public async Task<Repository> CreateAndClone(string repoName, Guid sourceProvider, Dictionary<string, object> sourceProviderConfig)
    {
        var availableServices = this.Services.GetServices<ISourceProviderService>();
        var selectedSourceProvider = availableServices.First(e => e.UUID == sourceProvider);
        
        var settings = this.SettingService.GetSettings();

        var repository = new Repository();
        repository.Name = repoName;
        repository.SourceProviderService = selectedSourceProvider.UUID;
        repository.SourceProviderConfiguration = sourceProviderConfig.AsJson();
        repository.Location = Path.Join(settings.Git.LocalFolder, repository.Name + "_" + Guid.NewGuid());
        
        this.Database.Repositories.Add(repository);
        this.Database.SaveChanges();


        if (!Directory.Exists(repository.Location))
        {
            Directory.CreateDirectory(repository.Location);
        }

        await selectedSourceProvider.CloneRepository(repository);
        
        return repository;
    }

    public Repository GetById(string id)
    {
        return this.GetById(Guid.Parse(id));
    }

    public Repository GetById(Guid id)
    {
        return this.Database.Repositories.First(e => e.Id == id);
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
        return this.Database.RepositoryLanguages.Include(e => e.Repository).Where(e => e.Repository.Id == repository.Id).ToList();
    }
}