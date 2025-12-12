using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Models;
using MRO.SMK.Docu.ApplicationCore.Services;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Guide;

public partial class Home : BaseRepoPage
{
    public Home(RepositoryService repositoryService) : base(repositoryService)
    {
        
    }
}