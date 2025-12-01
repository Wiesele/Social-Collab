using System.Web;
using BlazorMonaco.Editor;
using BlazorMonaco.Languages;
using BlazorMonaco;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MRO.SKM.Docu.Components.Layout;
using MRO.SKM.Docu.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Code;

public partial class Home : BaseRepoPage
{
    private IJSRuntime JSRuntime { get; set; }

    private FileListItem SelectedFile { get; set; }
    private string SelectedFileContent { get; set; }
    private StandaloneCodeEditor? Editor;

    public Home(RepositoryService repositoryService, IJSRuntime jsRuntime):base(repositoryService)
    {
        this.JSRuntime = jsRuntime;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private async Task FileSelected(FileListItem obj)
    {
        this.SelectedFile = obj;
        this.SelectedFileContent = await File.ReadAllTextAsync(this.SelectedFile.FullPath);
        await this.SetEditorValue();
    }

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            AutomaticLayout = true,
            ReadOnly = true,
        };
    }


    private async Task SetEditorValue()
    {
        if (this.Editor != null)
        {
            var model = await this.Editor.GetModel();

            var models = await BlazorMonaco.Editor.Global.GetModels(model.JsRuntime);
            var pathEncoded = this.SelectedFile.FullPath;
            TextModel? newModel = models.FirstOrDefault(e => HttpUtility.UrlDecode(e.Uri) == pathEncoded);
            if (newModel == null)
            {
                newModel = await BlazorMonaco.Editor.Global.CreateModel(model.JsRuntime,
                    this.SelectedFileContent,
                    null,
                    pathEncoded);
            }

            await this.Editor.SetModel(newModel);
        }
    }
}