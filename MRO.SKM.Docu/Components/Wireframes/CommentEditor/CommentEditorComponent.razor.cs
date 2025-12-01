using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.Comments;

namespace MRO.SKM.Docu.Components.Wireframes.CommentEditor;

public partial class CommentEditorComponent : ComponentBase
{
    private IJSRuntime JSRuntime { get; set; }
    private StandaloneCodeEditor? Editor;

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            AutomaticLayout = true,
            ReadOnly = true,
        };
    }

    [Parameter] public ILanguageProviderService LanguageProvider { get; set; }
    [Parameter] public ICommentable Comment { get; set; }
    [Parameter] public CodeFile File { get; set; }
    public Comment Model { get; set; } = new();
    public bool DisplayingMethod = true;

    public CommentEditorComponent(IJSRuntime jsRuntime)
    {
        this.JSRuntime = jsRuntime;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            this.DisplayingMethod = true;
            await this.SetEditorValue(this.Comment.Body);

            if (this.Comment.HasParameters)
            {
                this.Model.Params = this.LanguageProvider.GetParameterComments(this.Comment.Key, this.File.Key);
            }

            StateHasChanged();
        }
    }

    private async Task SetEditorValue(string text, bool skipTimeout = false)
    {
        if (!skipTimeout)
        {
            await Task.Delay(200);
        }

        if (this.Editor != null)
        {
            var model = await this.Editor.GetModel();

            var models = await BlazorMonaco.Editor.Global.GetModels(model.JsRuntime);

            models.ForEach(e => e.DisposeModel());

            var newModel = await BlazorMonaco.Editor.Global.CreateModel(model.JsRuntime,
                text,
                null,
                Guid.NewGuid() + this.LanguageProvider.FileExtension);

            await this.Editor.SetModel(newModel);
        }
    }

    private void AddException(MouseEventArgs obj)
    {
        this.Model.Exceptions.Add(new()
        {
        });
    }

    private async Task ToggleDisplayedCode()
    {
        if (this.DisplayingMethod)
        {
            await this.SetEditorValue(System.IO.File.ReadAllText(this.File.Key), true);
            this.DisplayingMethod = false;
        }            
        else
        {
            await this.SetEditorValue(this.Comment.Body, true);
            this.DisplayingMethod = true;
        }
    }
}