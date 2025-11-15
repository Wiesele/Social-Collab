using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MRO.SKM.Docu.ApplicationCore.Interfaces;
using MRO.SKM.Docu.Models.DynamicEditors;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Components.Pages.Repositories;

public partial class Add : ComponentBase
{
    private SettingService SettingService { get; set; }
    private LoaderService LoaderService { get; set; }
    private IDatabaseContext DatabaseContext { get; set; }
    private IServiceProvider Services { get; set; }

    public Repository Repository { get; set; } = new();
    public List<ISourceProviderService> SourceProviderServices { get; set; } = new();


    private ISourceProviderService? SelectedSourceProvider { get; set; } = null;
    private EditorConfiguration? SourceProviderEditorConfig { get; set; }

    private List<IDynamicEditorValue> SourceProviderEditorValue { get; set; }


    public Add(IServiceProvider services, IDatabaseContext databaseContext, LoaderService loaderService,
        SettingService settingService)
    {
        this.Services = services;
        this.LoaderService = loaderService;
        this.SettingService = settingService;
        this.DatabaseContext = databaseContext;
    }

    protected override void OnInitialized()
    {
        var providers = this.Services.GetServices<ISourceProviderService>();

        this.SourceProviderServices.AddRange(providers);

        base.OnInitialized();
    }

    private void OnSourceProviderChange(Guid selectedId)
    {
        var provider = this.SourceProviderServices.FirstOrDefault(e => e.UUID == selectedId);

        this.Repository.SourceProviderService = selectedId;
        this.SelectedSourceProvider = provider;

        if (this.SelectedSourceProvider != null)
        {
            var config = new EditorConfiguration();
            this.SelectedSourceProvider.GetEditorConfiguration(config);
            this.SourceProviderEditorConfig = config;
        }

        this.StateHasChanged();
    }

    private async Task AddNewRepository(MouseEventArgs arg)
    {
        var sourceProviderConifig = new Dictionary<string, object>();

        foreach (var item in this.SourceProviderEditorValue)
        {
            sourceProviderConifig.Add(item.Name, item.GetValue());
        }

        var settings = this.SettingService.GetSettings();

        var repository = new Repository();
        repository.Name = this.Repository.Name;
        repository.SourceProviderService = this.SelectedSourceProvider.UUID;
        repository.SourceProviderConfiguration = sourceProviderConifig.AsJson();
        repository.Location = Path.Join(settings.Git.LocalFolder, repository.Name + "_" + Guid.NewGuid());
        this.DatabaseContext.Repositories.Add(repository);
        this.DatabaseContext.SaveChanges();

        this.LoaderService.ShowLoader("Klone Repository");

        if (!Directory.Exists(repository.Location))
        {
            Directory.CreateDirectory(repository.Location);
        }

        await this.SelectedSourceProvider.CloneRepository(repository);

        this.LoaderService.HideLoader();
    }
}