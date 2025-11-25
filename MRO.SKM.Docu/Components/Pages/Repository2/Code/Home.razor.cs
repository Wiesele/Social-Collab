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

public partial class Home : ComponentBase
{
    private RepositoryService RepositoryService { get; set; }
    private IJSRuntime JSRuntime { get; set; }
    
    [Parameter]
    public string Id { get; set; }
    
    private Repository Repository { get; set; }
    private FileListItem SelectedFile { get; set; }
    private string SelectedFileContent { get; set; }
    private StandaloneCodeEditor? Editor;
    
    public Home(RepositoryService repositoryService, IJSRuntime jsRuntime)
    {
        this.JSRuntime = jsRuntime;
        this.RepositoryService = repositoryService;
    }

    protected override void OnInitialized()
    {
        this.Repository = this.RepositoryService.GetById(this.Id);
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
            Language = "c#",
            Value = ""
        };
    }


    private async Task SetEditorValue()
    {
        if (this.Editor != null)
        {
            var model = await this.Editor.GetModel();
            var languageId = "csharp";
            
            await BlazorMonaco.Editor.Global.SetModelLanguage(this.JSRuntime, model, languageId);
            
            await this.Editor.SetValue(this.SelectedFileContent);
        }
    }
}