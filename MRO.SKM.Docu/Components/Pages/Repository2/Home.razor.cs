using Microsoft.AspNetCore.Components;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Components.Pages.Repository2;

public partial class Home : ComponentBase
{
    private RepositoryService RepositoryService { get; set; }
    private RoslynService RoslynService { get; set; }
    
    
    public Home(RepositoryService repositoryService, RoslynService roslynService)
    {
        this.RepositoryService = repositoryService;
        this.RoslynService = roslynService;
    }
    
    [Parameter]
    public string Id { get; set; }
    
    public Repository? Repository { get; set; }


    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.Repository = this.RepositoryService.GetById(this.Id);
        
        this.StateHasChanged();

        this.RoslynService.Test(this.Repository);
    }
}