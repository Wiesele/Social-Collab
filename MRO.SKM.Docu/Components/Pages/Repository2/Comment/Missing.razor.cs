using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MudBlazor;

namespace MRO.SKM.Docu.Components.Pages.Repository2.Comment;

public partial class Missing : BaseRepoPage
{
    private readonly List<int> _items = Enumerable.Range(0, 51).ToList();

    public Missing(RepositoryService repositoryService) : base(repositoryService)
    {
        
    }
    
    private async Task<GridData<int>> ServerDataFunc(GridStateVirtualize<int> gridState, CancellationToken token)
    {
        try
        {
            var result = _items.ToList();

            await Task.Delay(1000, token);

            if (gridState.SortDefinitions.Count > 0) 
            {
                var firstSort = gridState.SortDefinitions.First();
                result = firstSort.Descending 
                    ? result.OrderByDescending(firstSort.SortFunc).ToList() 
                    : result.OrderBy(firstSort.SortFunc).ToList();
            }

            if (gridState.FilterDefinitions.Any())
            {
                var filterFunctions = gridState.FilterDefinitions.Select(x => x.GenerateFilterFunction());
                result = result
                    .Where(x => filterFunctions.All(f => f(x)))
                    .ToList();
            }

            var totalNumberOfFilteredItems = result.Count;
            
            result = result
                .Skip(gridState.StartIndex)
                .Take(gridState.Count)
                .ToList();

            return new GridData<int>
            {
                Items = result,
                TotalItems = totalNumberOfFilteredItems
            };
        }
        catch (TaskCanceledException)
        {
            return new GridData<int>
            {
                Items = [],
                TotalItems = 0
            };
        }
    }
}