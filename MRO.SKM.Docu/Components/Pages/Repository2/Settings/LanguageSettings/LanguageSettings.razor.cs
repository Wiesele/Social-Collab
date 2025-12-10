using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Models;
using MRO.SKM.SDK.Interfaces;
using MRO.SMK.Docu.ApplicationCore.Services;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Settings.LanguageSettings;

public partial class LanguageSettings : BaseRepoPage
{
    public IEnumerable<ILanguageProviderService> LanguageProviderServices { get; set; }
    
    [Parameter]
    public string LanguageServiceId { get; set; }
    public ILanguageProviderService  LanguageProviderService { get; set; }
    
    public LanguageSettings(RepositoryService repositoryService, IEnumerable<ILanguageProviderService> languageProviderService): base(repositoryService)
    {
        this.LanguageProviderServices = languageProviderService;
    }

    override protected async Task OnInitializedAsync()
    {
        this.LanguageProviderService =
            this.LanguageProviderServices.First(e => e.UUID == Guid.Parse(this.LanguageServiceId));
        
        base.OnInitializedAsync();
    }

    public async Task GenerateDocumentation()
    {
        var doc = await this.LanguageProviderService.GenerateHtmlDocumentation(this.Repository);
    }
}