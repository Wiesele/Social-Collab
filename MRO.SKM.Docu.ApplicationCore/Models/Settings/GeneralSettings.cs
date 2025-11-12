using System.Runtime.CompilerServices;

namespace MRO.SMK.Docu.ApplicationCore.Models.Settings;

public class GeneralSettings
{
    public  GeneralSettings()
    {
        this.Git = new();
    }
    
    public GitSettings Git { get; set; }
}