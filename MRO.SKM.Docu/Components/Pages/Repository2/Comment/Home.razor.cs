using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Infrastructure;
using MRO.SKM.Docu.Models;
using MRO.SMK.Docu.ApplicationCore.Services;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Comment;

public partial class Home : BaseRepoPage
{
    public Home(RepositoryService repositoryService) : base(repositoryService)
    {
        
    }
}