using AjaxControlToolkit.HtmlEditor;

/// <summary>
/// Summary description for CustomEditor
/// </summary>
namespace MyControls
{
    public class CustomEditor : AjaxControlToolkit.HtmlEditor.Editor
    {
        protected override void FillTopToolbar()
        {
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.Bold());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.Italic());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.Underline());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.JustifyLeft());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.JustifyCenter());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.JustifyRight());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.OrderedList());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.BulletedList());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.IncreaseIndent());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.DecreaseIndent());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.Undo());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.Redo());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.InsertLink());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.RemoveLink());
            //TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.PasteText());
            //TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.PasteWord());
            //Missing Speller Checker
        }

        protected override void FillBottomToolbar()
        {
            BottomToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.DesignMode());
            BottomToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.HtmlMode());
            BottomToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.PreviewMode());
        }
    }

    public class CustomEditorNoToolbar : CustomEditor
    {
        protected override void FillTopToolbar()
        {
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.DesignMode());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.HtmlMode());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.PreviewMode());
        }
        protected override void FillBottomToolbar()
        {
            BottomToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.DesignMode());
            BottomToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.HtmlMode());
            BottomToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButtons.PreviewMode());
        }
    }
}