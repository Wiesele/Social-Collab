using Microsoft.AspNetCore.Components;

namespace MRO.SMK.Docu.ApplicationCore.Services;

public class LoaderService
{
    public bool Visible { get; set; } = false;
    public string Text { get; set; } = string.Empty;

    public EventHandler<bool> VisibilityChanged { get; set; }
    public EventHandler<string> TextChanged { get; set; }

    public void ShowLoader(string text = "")
    {
        if (this.Visible == false)
        {
            this.Visible = true;
            this.VisibilityChanged.Invoke(this, this.Visible);
        }

        if (this.Text != text)
        {
            this.Text = text;
            this.TextChanged.Invoke(this, this.Text);
        }
    }

    public void HideLoader()
    {
        this.Visible = false;
    }
}