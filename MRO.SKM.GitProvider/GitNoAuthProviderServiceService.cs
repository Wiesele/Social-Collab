using MRO.SKM.GitProvider.Models;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;

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
    }
}