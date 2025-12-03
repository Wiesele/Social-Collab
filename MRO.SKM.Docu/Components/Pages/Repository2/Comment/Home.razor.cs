using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Components.Wireframes.CommentEditor;
using MRO.SKM.Docu.Infrastructure;
using MRO.SKM.Docu.Models;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MudBlazor;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Comment;

public partial class Home : BaseRepoPage
{
    private CodeService CodeService { get; set; }
    private IDialogService DialogService { get; set; }
    private ISnackbar Snackbar { get; set; }

    private FileListItem SelectedFile { get; set; }

    private List<Class> Classes { get; set; } = new List<Class>();


    public Home(RepositoryService repositoryService,
        CodeService codeService,
        IDialogService dialogService,
        ISnackbar snackbar) : base(repositoryService)
    {
        this.CodeService = codeService;
        this.DialogService = dialogService;
        this.Snackbar = snackbar;
    }


  
    private async Task FileSelected(FileListItem obj)
    {
        this.SelectedFile = obj;

        if (this.SelectedFile.IsFile)
        {
            this.Classes = this.CodeService.GetClassesInFile(this.SelectedFile.FullPath);
        }
    }

    private Task OpenCommentDialog(ICommentable? item)
    {
        if (item == null)
        {
            return Task.CompletedTask;
        }

        var provider = this.CodeService.GetLanguageproviderForFile(this.SelectedFile.FullPath);

        if (provider == null)
        {
            this.Snackbar.Add("Keine Sprache für '" + Path.GetExtension(this.SelectedFile.FullPath) + "' konfiguriert", Severity.Warning);
        }
        
        var options = new DialogOptions()
        {
            FullScreen = true,
            CloseButton = true,
        };

        var parameters = new DialogParameters<CommentEditorDialog>
        {
            { x => x.Item, item },
            { x => x.LanguageProviderService, provider },
            { x => x.File, this.RepositoryService.GetCodeFile(this.SelectedFile.FullPath)}
        };

        return DialogService.ShowAsync<CommentEditorDialog>("Kommentar bearbeiten", parameters, options);
    }
}