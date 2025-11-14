using MRO.SKM.SDK.Models.Editors;

namespace MRO.SKM.SDK.Models;

public class EditorConfiguration
{
    public List<BaseEditor> Editors { get; set; }
    
    public EditorConfiguration()
    {
        this.Editors = new List<BaseEditor>();
    }

    public void AddTextbox(Action<TextEditorConfiguration> createTextBoxEditor)
    {
        var box = new TextEditorConfiguration();
        
        createTextBoxEditor(box);
        
        this.ApplyEditorDefaults(box);
        
        this.Editors.Add(box);
    }
    public void AddPasswordBox(Action<PasswordEditorConfiguration> createTextBoxEditor)
    {
        var box = new PasswordEditorConfiguration();
        
        createTextBoxEditor(box);
        
        this.ApplyEditorDefaults(box);

        this.Editors.Add(box);
    }
    
    public void AddCheckbox(Action<CheckBoxEditorConfiguration> createTextBoxEditor)
    {
        var box = new CheckBoxEditorConfiguration();
        
        createTextBoxEditor(box);
        
        this.ApplyEditorDefaults(box);
        
        this.Editors.Add(box);
    }

    private void ApplyEditorDefaults(BaseEditor editor)
    {
        if (editor.Index == 0)
        {
            editor.Index = this.Editors.Count;
        }

        if (string.IsNullOrWhiteSpace(editor.Name))
        {
            editor.Name = editor.Lable;
        }
        
        if (string.IsNullOrWhiteSpace(editor.Lable))
        {
            editor.Lable = editor.Name;
        }
    }
}