using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MRO.SMK.Docu.ApplicationCore.Constants;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;


namespace MRO.SKM.Docu.Components.Layout;

public partial class RepositoryFullPageLayout: LayoutComponentBase
{
    private ProtectedSessionStorage ProtectedSessionStore { get; set; }
    private RepositoryService RepositoryService { get; set; }
    
    private string RepositoryId { get; set; }
    private Repository SelectedRepository { get; set; }

    public bool UseFullPage = false;

    public RepositoryFullPageLayout(RepositoryService repositoryService, ProtectedSessionStorage protectedSessionStore)
    {
        this.RepositoryService = repositoryService;
        this.ProtectedSessionStore = protectedSessionStore;
    }
    

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var result = await ProtectedSessionStore.GetAsync<string>(SessionStorageKeys.RepositoryId);

            if (result.Success)
            {
                this.RepositoryId = result.Value;
                this.SelectedRepository = this.RepositoryService.GetById(this.RepositoryId);
                this.StateHasChanged();
            }
        }
        
        base.OnAfterRenderAsync(firstRender);
    }
}