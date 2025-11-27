
namespace MRO.SMK.Docu.ApplicationCore.Services;

public class LoaderService
{
    public bool Visible { get; set; } = false;
    public string Text { get; set; } = string.Empty;
    private Action CallBack { get; set; }
    
    public async Task ShowLoader(string text = "")
    {
        if (this.Visible == false)
        {
            this.Visible = true;
        }

        if (this.Text != text)
        {
            this.Text = text;
        }
        
        this.CallBack?.Invoke();
        
        await Task.Delay(50);
    }

    public void RegisterCallback(Action callback)
    {
        this.CallBack = callback;
    }
    
    public void HideLoader()
    {
        this.Visible = false;
        this.CallBack?.Invoke();
    }       

}