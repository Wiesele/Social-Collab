using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models.SourceControl;
using MRO.SMK.Docu.ApplicationCore.Extensions;
using MRO.SMK.SDK.Models;

namespace MRO.SMK.Docu.ApplicationCore.Services;

public class SourceControlService
{
    private IEnumerable<ISourceProviderService> SourceProviderServices { get; set; }

    public SourceControlService(IEnumerable<ISourceProviderService> sourceProviderServices)
    {
        this.SourceProviderServices = sourceProviderServices;
    }

    public async Task CloneRepository(Repository repository)
    {
        if (!Directory.Exists(repository.Location))
        {
            Directory.CreateDirectory(repository.Location);
        }

        var selectedSourceProvider = this.SourceProviderServices.GetSourceProvider(repository);

        await selectedSourceProvider.CloneRepository(repository);
    }

    public ISourceProviderService GetSourceProvider(Guid uuid)
    {
        return this.SourceProviderServices.GetSourceProvider(uuid);
    }

    public async Task<List<Branch>> ListBranches(Repository repository)
    {
        var provider = this.SourceProviderServices.GetSourceProvider(repository);
        var branches = await provider.ListBranches(repository);
        
        return branches;
    }

    public async Task ChangeBranch(Repository repository, Branch branch)
    {
        var provider = this.SourceProviderServices.GetSourceProvider(repository);
        await provider.ChangeBranch(repository, branch);
    } 
    
    public async Task UpdateRepository(Repository repository)
    {
        var provider = this.SourceProviderServices.GetSourceProvider(repository);
        await provider.UpdateRepository(repository);
    } 
}