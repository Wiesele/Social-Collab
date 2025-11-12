using Microsoft.Extensions.Configuration;
using MRO.SMK.Docu.ApplicationCore.Models.Settings;

namespace MRO.SMK.Docu.ApplicationCore.Services;

public class SettingService
{
    private IConfiguration Configuration { get; set; }

    public SettingService(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public GeneralSettings GetSettings()
    {
        var settings = new GeneralSettings();
        
        settings.Git.LocalFolder = this.Configuration["Git:LocalFolder"];
        
        return settings;
    }
}