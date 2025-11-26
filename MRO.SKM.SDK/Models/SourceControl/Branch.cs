namespace MRO.SKM.SDK.Models.SourceControl;

public class Branch
{
    public string DisplayName { get; set; }
    public string Id { get; set; }
    public bool IsRemote { get; set; }
    public bool IsCurrentlyActive { get; set; }
}