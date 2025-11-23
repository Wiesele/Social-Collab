using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MRO.SKM.SDk.Extensions;
using MRO.SMK.Docu.ApplicationCore.Constants;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Components.Layout;

public partial class RepositoryNavMenu : ComponentBase
{
    private ProtectedSessionStorage ProtectedSessionStore { get; set; }

    public RepositoryNavMenu(ProtectedSessionStorage protectedSessionStore)
    {
        this.ProtectedSessionStore = protectedSessionStore;
    }
    
    [Parameter]
    public Repository? Repository { get; set; }

    public string GetUrl(string subPath)
    {
        if (subPath.IsNullOrWhiteSpace())
        {
            return "/repository/" + this.Repository!.Id;
        }
        
        return "/repository/" + this.Repository!.Id + "/" + subPath;
    }

}
