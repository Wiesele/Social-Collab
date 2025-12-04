using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Components.Pages.Repository2.Settings.LanguageSettings;
using MRO.SKM.Docu.Models;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MudBlazor;
using MRO.SKM.Docu.Extensions;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Settings.AiSettings;

public partial class Home : BaseRepoPage
{
    private List<ILanguageModelService> LanguageModelServices { get; set; }
    private IDialogService  DialogService { get; set; }
    private List<RepositoryAiConfiguration> AiModels { get; set; } = new();

    public Home(IEnumerable<ILanguageModelService> languageModelServices, 
        IDialogService  dialogService, 
        RepositoryService repositoryService): base(repositoryService)
    {
        this.LanguageModelServices = languageModelServices.ToList();
        this.DialogService = dialogService;
        this.RepositoryService = repositoryService;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        this.AiModels = this.RepositoryService.ListAiModels(this.Repository);
    }       


    public async Task OpenAddLanguageDialog()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var parameter = new DialogParameters<AddLanguageModelDialog>()
        {
            { x => x.AlreadyAdded, this.AiModels }
        };

        var result = await DialogService.ShowAsync<AddLanguageModelDialog>("Simple Dialog", parameter, options);
        var dialogRes = await result.Result;

        if (dialogRes != null && !dialogRes.Canceled)
        {
            var providerGuid = (AddLanguageModelDialogReturnValue)dialogRes.Data;

            var sourceProviderConifig = providerGuid.Config.ToDictionary();
            
            this.RepositoryService.AddAiModel(this.Repository, providerGuid.UUID, sourceProviderConifig);
            this.AiModels = this.RepositoryService.ListAiModels(this.Repository);
            
            this.StateHasChanged();
        }
    }

    private void NavigateTo(Guid lmId)
    {
        this.NavigationManager.NavigateTo("/repository/" + this.Id + "/settings/ai/" + lmId.ToString());
    }
}