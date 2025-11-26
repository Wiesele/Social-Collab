using LibGit2Sharp;
using MRO.SKM.GitProvider.Extensions;
using MRO.SKM.GitProvider.Models;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using Branch = MRO.SKM.SDK.Models.SourceControl.Branch;
using Repository = MRO.SMK.SDK.Models.Repository;

namespace MRO.SKM.GitProvider;

public class GitNoAuthProviderServiceService : ISourceProviderService
{
    public Guid UUID { get; } = Guid.Parse("D3A94367-183A-4F2A-9202-E54C7503927A");
    public string DisplayName { get; } = "Git (Keine Authentifizierung)";


    public void GetEditorConfiguration(EditorConfiguration config)
    {
        config.AddTextbox(e =>
        {
            e.Name = nameof(GitNoAuthData.RepoUrl);
            e.Lable = "URL";
        });
        config.AddTextbox(e =>
        {
            e.Name = nameof(GitNoAuthData.Username);
            e.Lable = "Benutzer";
        });
        config.AddTextbox(e =>
        {
            e.Name = nameof(GitNoAuthData.Email);
            e.Lable = "E-Mail";
        });
    }

    public async Task CloneRepository(Repository repository)
    {
        var configuration = repository.SourceProviderConfiguration.ParseAsJson<GitNoAuthData>();

        LibGit2Sharp.Repository.Clone(configuration.RepoUrl, repository.Location);
    }

    private void Fetch(LibGit2Sharp.Repository repo)
    {
        string logMessage = "";
        var remote = repo.Network.Remotes["origin"];
        var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
        Commands.Fetch(repo, remote.Name, refSpecs, null, logMessage);
    }
    
    public async Task<List<Branch>> ListBranches(Repository repository)
    {
        var data = new List<Branch>();
        
        using (var repo = new LibGit2Sharp.Repository(repository.Location))
        {
            this.Fetch(repo);
            
            foreach(var branch in repo.Branches)
            {
                var branchData = new Branch();
                branchData.DisplayName = branch.FriendlyName;
                branchData.Id = branch.CanonicalName;
                branchData.IsRemote = branch.IsRemote;
                branchData.IsCurrentlyActive = branch.IsCurrentRepositoryHead;

                if (!branchData.DisplayName.Contains("HEAD"))
                {
                    data.Add(branchData);
                }
            }
        }
        
        return data;
    }

    public async Task ChangeBranch(Repository repository, Branch branch)
    {
        using (var repo = new LibGit2Sharp.Repository(repository.Location))
        {
            this.Fetch(repo);
            
            var selectedBranch = repo.Branches.FirstOrDefault(e => e.CanonicalName == branch.Id);
            
            var localBranchName = selectedBranch.FriendlyName.ToLocalBranchName();

            // Prüfen ob der Branch bereits lokal vorhanden ist
            if (repo.Branches.Any(b => b.FriendlyName == localBranchName))
            {
                Commands.Checkout(repo, repo.Branches.First(b => b.FriendlyName == localBranchName));
            }
            else
            {
                // Referenz: https://stackoverflow.com/a/46605523
                // Lokalen Branch auf Basis des Remotes erstellen
                var localBranch = repo.CreateBranch(localBranchName, selectedBranch.Tip);
            
                // Tracking zwischen lokalem und remote Branch einstellen
                repo.Branches.Update(localBranch, b => b.UpstreamBranch = selectedBranch.CanonicalName);
                repo.Branches.Update(localBranch, b => b.TrackedBranch = selectedBranch.CanonicalName);
            
                var currentBranch = Commands.Checkout(repo, localBranch);
            }
            
            this.Fetch(repo);
        }
    }

    public async Task UpdateRepository(Repository repository)
    {
        var configuration = repository.SourceProviderConfiguration.ParseAsJson<GitNoAuthData>();

        using (var repo = new LibGit2Sharp.Repository(repository.Location))
        {
            LibGit2Sharp.PullOptions options = new LibGit2Sharp.PullOptions();
            options.FetchOptions = new FetchOptions();
            // User information to create a merge commit
            var signature = new LibGit2Sharp.Signature(
                new Identity(configuration.Username, configuration.Email), DateTimeOffset.Now);

            Commands.Pull(repo, signature, options);
        }
    }
}