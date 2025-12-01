using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Components.Pages.Repository2;

public partial class Home : BaseRepoPage
{
    public Home(RepositoryService repositoryService):base(repositoryService)
    {
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        this.StateHasChanged();
    }
}