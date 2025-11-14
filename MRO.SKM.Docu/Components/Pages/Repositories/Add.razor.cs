using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MRO.SKM.Docu.Models.DynamicEditors;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.Docu.ApplicationCore.Extensions;
using MRO.SMK.Docu.ApplicationCore.Models;

namespace MRO.SKM.Docu.Components.Pages.Repositories;

public partial class Add : ComponentBase
{
    public Repository Repository { get; set; } = new();
    public List<ISourceProviderService> SourceProviderServices { get; set; } = new();

    private IServiceProvider Services { get; set; }

    private ISourceProviderService? SelectedSourceProvider { get; set; } = null;
    private EditorConfiguration? SourceProviderEditorConfig { get; set; }
    
    private List<IDynamicEditorValue> SourceProviderEditorValue { get; set; }
    
    public Add(IServiceProvider services)
    {
        this.Services = services;
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
        
        
        var repository = new Repository();
        repository.Name = this.Repository.Name;
        repository.SourceProviderService = this.SelectedSourceProvider.UUID;
        repository.SourceProviderConfiguration = sourceProviderConifig.AsJson();
    }
}   
