using MudBlazor;

namespace MRO.SKM.Docu.Models;

public class FileListItem
{
    public string DisplayName { get; set; }
    public string Icon { get; set; }
    public string IconOpen { get; set; }
    public string FullPath { get; set; }
    public bool IsFile { get; set; } = false;
}