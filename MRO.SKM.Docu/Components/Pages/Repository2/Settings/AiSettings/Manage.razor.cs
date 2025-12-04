using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Models;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MudBlazor;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Settings.AiSettings;

public partial class Manage : BaseRepoPage
{
    public RepositoryAiConfiguration Config { get; set; } = new();
    public IEnumerable<ILanguageModelService> LanguageModels { get; set; }
    public ILanguageModelService LanguageModel { get; set; }
    public ISnackbar Snackbar { get; set; }

    [Parameter]
    public string LLMConfigId { get; set; }
    
    public Manage(RepositoryService repositoryService, 
        IEnumerable<ILanguageModelService> languageModelModelServices,
            ISnackbar snackbar) : base(repositoryService)
    {
        this.LanguageModels = languageModelModelServices;
        this.Snackbar = snackbar;
    }


    protected override Task OnInitializedAsync()
    {
        this.Config = this.RepositoryService.GetAiConfig(this.LLMConfigId);
        this.LanguageModel = this.LanguageModels.First(e => e.UUID == this.Config.ProviderId);
        
        return base.OnInitializedAsync();
    }

    private void SaveChanges()
    {
        this.RepositoryService.UpdateAiConfig(this.Config);
        this.Snackbar.Add(this.LanguageModel.DisplayName + " aktualisiert", Severity.Success);
    }
}