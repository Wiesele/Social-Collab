using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MRO.SKM.Docu.Infrastructure;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Models;

public abstract class BaseRepoPage: ComponentBase
{
    public BaseRepoPage(RepositoryService repositoryService)
    {
        this.RepositoryService = repositoryService;
    }

    protected RepositoryService RepositoryService { get; set; }
    
    [Parameter] 
    public string Id { get; set; }
    
    protected Repository Repository { get; set; }
    
    protected override void OnInitialized()
    {
        this.Repository  = this.RepositoryService.GetById(this.Id);
        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        this.Repository = this.RepositoryService.GetById(this.Id);
        await base.OnInitializedAsync();
    }
}