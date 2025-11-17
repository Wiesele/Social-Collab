using System.Runtime.CompilerServices;
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
    private LoaderService LoaderService { get; set; }
    private IServiceProvider Services { get; set; }
    private NavigationManager NavigationManager { get; set; }
    private RepositoryService RepositoryService { get; set; }

    public Repository Repository { get; set; } = new();
    public List<ISourceProviderService> SourceProviderServices { get; set; } = new();


    private ISourceProviderService? SelectedSourceProvider { get; set; } = null;
    private EditorConfiguration? SourceProviderEditorConfig { get; set; }

    private List<IDynamicEditorValue> SourceProviderEditorValue { get; set; }


    public Add(IServiceProvider services, 
        IDatabaseContext databaseContext, 
        LoaderService loaderService,
        SettingService settingService,
        NavigationManager navigationManager,
        RepositoryService repositoryService)
    {
        this.Services = services;
        this.LoaderService = loaderService;
        this.NavigationManager = navigationManager;
        this.RepositoryService = repositoryService;
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
        this.LoaderService.ShowLoader("Klone Repository");

        var sourceProviderConifig = new Dictionary<string, object>();

        foreach (var item in this.SourceProviderEditorValue)
        {
            sourceProviderConifig.Add(item.Name, item.GetValue());
        }

        var repo = await this.RepositoryService.CreateAndClone(this.Repository.Name, this.SelectedSourceProvider.UUID, sourceProviderConifig);

        this.LoaderService.HideLoader();
        
        NavigationManager.NavigateTo("/repository/" + repo.Id);
    }
}