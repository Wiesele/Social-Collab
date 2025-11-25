using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Models;
using MudBlazor;

namespace MRO.SKM.Docu.Components.Wireframes;

public partial class FileList : ComponentBase
{
    [Parameter] public string BasePath { get; set; }
    public List<TreeItemData<FileListItem>> InitialTreeView { get; set; } = new();
    
    [Parameter]
    public EventCallback<FileListItem> FileSelected { get; set; }


    protected override void OnInitialized()
    {
        base.OnInitialized();

        var items = this.GetTreeItems(BasePath);
        this.InitialTreeView.AddRange(items);
    }

    public async Task<IReadOnlyCollection<TreeItemData<FileListItem>>> LoadServerData(FileListItem parentValue)
    {
        var items = this.GetTreeItems(parentValue.FullPath);

        return items;
    }

    public class FilePickerPresenter : TreeItemData<FileListItem>
    {
        public string IconExpanded { get; set; }
        public bool CanExpand { get; set; }

        public FilePickerPresenter(FileListItem item) : base(item)
        {
            this.Text = item.DisplayName;
            this.Icon = item.Icon;
            this.IconExpanded = item.IconOpen;
            this.CanExpand = !item.IsFile;
        }
    }

    private List<TreeItemData<FileListItem>> GetTreeItems(string currentPath)
    {
        var folderContents = this.GetFileListItems(currentPath);
        var itemPresenters = new List<TreeItemData<FileListItem>>();

        foreach (var child in folderContents)
        {
            var presenter = new FilePickerPresenter(child);
            itemPresenters.Add(presenter);
        }

        var ordered = itemPresenters
            .OrderBy(e => e.Value.IsFile)
            .ThenBy(e => e.Text)
            .ToList();

        return ordered;
    }


    private List<FileListItem> GetFileListItems(string path)
    {
        var files = Directory.GetFileSystemEntries(path);
        var lst = new List<FileListItem>();
        foreach (var item in files)
        {
            var entry = new FileListItem()
            {
                DisplayName = Path.GetFileName(item),
                FullPath = item,
            };

            var atts = File.GetAttributes(item);
            if (atts.HasFlag(FileAttributes.Directory))
            {
                entry.IsFile = false;
                entry.Icon = Icons.Material.Filled.Folder;
                entry.IconOpen = Icons.Material.Filled.FolderOpen;
            }
            else
            {
                entry.IsFile = true;
            }

            lst.Add(entry);
        }


        return lst;
    }
    
    private void FileChanged(FileListItem obj)
    {
        if (obj.IsFile)
        {
            this.FileSelected.InvokeAsync(obj);
        }
    }
}