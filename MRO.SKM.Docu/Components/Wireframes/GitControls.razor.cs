using Microsoft.AspNetCore.Components;
using MRO.SKM.SDK.Models.SourceControl;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Components.Wireframes;

public partial class GitControls : ComponentBase
{
    private RepositoryService RepositoryService { get; set; } 
    private SourceControlService SourceControlService { get; set; } 

    
    [Parameter]
    public Repository Repository { get; set; }
    
    public List<Branch> Branches { get; set; } = new List<Branch>();
    public Branch? SelectedBranch { get; set; }
    public bool IsBranchSelectOpen { get; set; }
    public LoaderService LoaderService { get; set; }

    
    public GitControls(RepositoryService repositoryService,
        SourceControlService sourceControlService,
        LoaderService loaderService)
    {
        this.SourceControlService = sourceControlService;
        this.RepositoryService = repositoryService;
        this.LoaderService = loaderService;
    }


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await this.FetchData();
    }

    public void OpenBranchesClick()
    {
        if (this.IsBranchSelectOpen)
        {
            this.IsBranchSelectOpen = false;
        }
        else
        {
            this.IsBranchSelectOpen = true;
        }
    }

    public async Task FetchData()
    {
        this.Branches = await this.SourceControlService.ListBranches(this.Repository);
        this.SelectedBranch = this.Branches.FirstOrDefault(e => e.IsCurrentlyActive);
        
        this.StateHasChanged();
    }

    public async Task UpdateRepositoryClick()
    {
        await this.LoaderService.ShowLoader("Pulle von Remote");
        
        await this.SourceControlService.UpdateRepository(this.Repository);
        
        this.LoaderService.HideLoader();
    }
    
    public async void OnBranchSelectionChanged(Branch branch)
    {
        await this.SourceControlService.ChangeBranch(this.Repository, branch);

        await this.FetchData();
    }
}