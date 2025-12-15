using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MRO.SKM.Docu.Models;
using MRO.SMK.Docu.ApplicationCore.Services;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Guide;

public partial class Home : BaseRepoPage
{
    private IJSRuntime JsRuntime { get; set; }

    public Home(RepositoryService repositoryService, IJSRuntime jsRuntime) : base(repositoryService)
    {
        this.JsRuntime = jsRuntime;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await this.JsRuntime.InvokeVoidAsync("autosizeIframe", "styleIframe");
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}