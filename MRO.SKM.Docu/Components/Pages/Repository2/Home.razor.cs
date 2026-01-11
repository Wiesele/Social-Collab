using Microsoft.AspNetCore.Components;
using MRO.SKM.Docu.Models;
using MRO.SMK.Docu.ApplicationCore.Models;
using MRO.SMK.Docu.ApplicationCore.Services;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.Docu.Components.Pages.Repository2;

public partial class Home : BaseRepoPage
{
    public Home(RepositoryService repositoryService) : base(repositoryService)
    {
    }

    public List<CoverageChartData> Coverage = new List<CoverageChartData>();
    public List<string> Labels = new List<string>();
    public List<double> Data = new List<double>();

    public double[] DataArray
    {
        get { return Data.ToArray(); }
    }
    public string[] LabelsArray
    {
        get { return Labels.ToArray(); }
    }


    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.Coverage = this.RepositoryService.GetCoverageChartData(this.Id);

        foreach (var item in this.Coverage)
        {
            this.Labels.Add(item.DisplayName);
            this.Data.Add(item.Count);
        }

        this.StateHasChanged();
    }
}