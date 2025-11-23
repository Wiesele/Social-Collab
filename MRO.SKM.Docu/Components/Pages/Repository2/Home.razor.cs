using Microsoft.AspNetCore.Components;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Components.Pages.Repository2;

public partial class Home : ComponentBase
{
    private RepositoryService RepositoryService { get; set; }
    
    
    public Home(RepositoryService repositoryService)
    {
        this.RepositoryService = repositoryService;
    }
    
    [Parameter]
    public string Id { get; set; }
    
    public Repository? Repository { get; set; }


    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.Repository = this.RepositoryService.GetById(this.Id);
        
        this.StateHasChanged();
    }
}