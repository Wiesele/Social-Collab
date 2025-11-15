using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.GitProvider;

public class GitHubSourceProviderService : ISourceProviderService
{
    public Guid UUID { get; } = Guid.Parse("DBDC19D3-7415-4746-9FA5-DD929B954197");
    public string DisplayName { get; } = "Github";
    
    
    public void GetEditorConfiguration(EditorConfiguration config)
    {
        config.AddTextbox(e =>
        {
            e.Name = "URL";
            e.Lable = "URLLable";
        });
        config.AddCheckbox(e =>
        {
            e.Name = "TestCheck";
            e.Lable = "TestCheckLable";
            e.Index = 999;
        });
        config.AddTextbox(e =>
        {
            e.Name = "Passwd";
            e.Lable = "ddddLable";
        });
    }

    public Task CloneRepository(Repository repository)
    {
        throw new NotImplementedException();
    }
}