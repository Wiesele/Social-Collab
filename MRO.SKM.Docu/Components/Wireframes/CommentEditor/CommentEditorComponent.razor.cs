using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MRO.SKM.Docu.Components.Wireframes.SimpleDialogs;
using MRO.SKM.Google.Gemini;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.Comments;
using MRO.SKM.SDK.Models.LanaugeModels;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;
using MudBlazor;

namespace MRO.SKM.Docu.Components.Wireframes.CommentEditor;

public partial class CommentEditorComponent : ComponentBase
{
    private IJSRuntime JSRuntime { get; set; }
    private IDialogService DialogService { get; set; }
    private ISnackbar Snackbar { get; set; }
    private RepositoryService RepositoryService { get; set; }
    private LanguageModelService LanguageModelService { get; set; }
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
    [Parameter] public EventCallback<bool> CommentEdited { get; set; }
    private LmRepositoryFeatures RepositoryFeatures { get; set; } = new();
    public Comment AiComment { get; set; } = new();
    public Comment Model { get; set; } = new();
    public bool DisplayingMethod = true;
    private Repository Repository { get; set; } 
    private bool AutoLoadAiSuggestions { get; set; }
    private bool AiSuggestionsLoaded { get; set; }

    public CommentEditorComponent(IJSRuntime jsRuntime,
        IDialogService dialogService,
        ISnackbar snackbar,
        RepositoryService repositoryService,
        LanguageModelService languageModelService)
    {
        this.LanguageModelService = languageModelService;
        this.DialogService = dialogService;
        this.JSRuntime = jsRuntime;
        this.Snackbar = snackbar;
        this.RepositoryService = repositoryService;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            this.DisplayingMethod = true;
            await this.SetEditorValue(await this.LanguageProvider.GetObjectBody(this.Comment.Key, this.File.Key));

            this.Model = await this.LanguageProvider.AnalyzeComment(this.Comment.Key, this.File.Key);
            this.Repository = this.RepositoryService.GetById(this.File.RepositoryId);

            this.RepositoryFeatures = this.LanguageModelService.GetRepositoryFeatures(this.Repository);

            if (this.Model.Summary.IsNullOrWhiteSpace())
            {
                this.AutoLoadAiSuggestions = true;
                this.LoadAiSuggestions();
            }
            else
            {
                this.AutoLoadAiSuggestions = false;
            }
            
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
        
    }

    private void LazyLoadAiSuggestions()
    {
        if (!this.AiSuggestionsLoaded)
        {
            this.LoadAiSuggestions();
        }
    }

    private void LoadAiSuggestions()
    {
        if (!this.RepositoryFeatures.GenerateDoc)
        {
            return;
        }
        
        this.AiSuggestionsLoaded = true;
        this.InvokeAsync(async () =>
        {
            this.AiComment = await this.LanguageModelService.GenerateDocumentation(this.Repository, this.Comment);
            this.StateHasChanged();
        });
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
            await this.SetEditorValue(await System.IO.File.ReadAllTextAsync(this.File.Key), true);
            this.DisplayingMethod = false;
        }
        else
        {
            await this.SetEditorValue(await this.LanguageProvider.GetObjectBody(this.Comment.Key, this.File.Key));
            this.DisplayingMethod = true;
        }
    }

    public async Task SaveChanges()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };

        var dialog = await DialogService.ShowAsync<SimpleConfirmationDialog>("Speichern Bestätigen", options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await this.LanguageProvider.AddTriviaToMethod(this.Comment.Key, this.File.Key, this.Model);
            this.Snackbar.Add("Kommentar zu '" + this.Comment.Name + "' hinzugefügt", Severity.Success);
            await this.CommentEdited.InvokeAsync(true);
        }
    }
}