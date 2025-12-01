using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Models;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;
using MudBlazor;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Settings.LanguageSettings;

public partial class Home : BaseRepoPage
{
    private List<ILanguageProviderService> LanguageProviderServices { get; set; }
    private IDialogService  DialogService { get; set; }
    private List<RepositoryLanguage> Languages { get; set; } = new();

    public Home(IEnumerable<ILanguageProviderService> languageProviderServices, 
        IDialogService  dialogService, 
        RepositoryService repositoryService): base(repositoryService)
    {
        this.LanguageProviderServices = languageProviderServices.ToList();
        this.DialogService = dialogService;
        this.RepositoryService = repositoryService;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        this.Languages = this.RepositoryService.ListLanguages(this.Repository);
    }       


    public async Task OpenAddLanguageDialog()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var parameter = new DialogParameters<AddLanguageDialog>()
        {
            { x => x.AlreadyAdded, this.Languages }
        };

        var result = await DialogService.ShowAsync<AddLanguageDialog>("Simple Dialog", parameter, options);
        var dialogRes = await result.Result;

        if (dialogRes != null && !dialogRes.Canceled)
        {
            var providerGuid = (Guid)dialogRes.Data;
            this.RepositoryService.AddLanguage(this.Repository, providerGuid);
            this.Languages = this.RepositoryService.ListLanguages(this.Repository);
            
            this.StateHasChanged();
        }
    }
}