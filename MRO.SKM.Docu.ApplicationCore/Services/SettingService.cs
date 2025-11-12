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

    private void GetSettingsDictionaryInternal(List<SettingValue> data,
        string section = null)
    {
        var children = Configuration.GetChildren();

        if (section != null)
        {
            children = Configuration.GetSection(section).GetChildren();
        }

        foreach (var item in children)
        {
            var key = item.Key;

            if (section != null)
            {
                key = section + ":" + key;
            }

            if (item.Value != null)
            {
                data.Add(new()
                {
                    Value = item.Value,
                    Name = key
                });
            }
            else
            {
                this.GetSettingsDictionaryInternal(data, key);
            }
        }
    }

    public List<SettingValue> GetSettingsDictionary(string section = null)
    {
        var data = new List<SettingValue>();

        this.GetSettingsDictionaryInternal(data, section);

        return data;
    }
}