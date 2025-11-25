using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Components.Layout;
using MRO.SKM.Docu.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Code;

public partial class Home : ComponentBase
{
    private RepositoryService RepositoryService { get; set; }
    [Parameter]
    public string Id { get; set; }
    
    private Repository Repository { get; set; }
    private FileListItem SelectedFile { get; set; }
    private string SelectedFileContent { get; set; }
    
    public Home(RepositoryService repositoryService)
    {
        this.RepositoryService = repositoryService;
    }

    protected override void OnInitialized()
    {
        this.Repository = this.RepositoryService.GetById(this.Id);
        base.OnInitialized();
    }
    
    private void FileSelected(FileListItem obj)
    {
        this.SelectedFile = obj;
        this.SelectedFileContent = File.ReadAllText(this.SelectedFile.FullPath);
    }
}