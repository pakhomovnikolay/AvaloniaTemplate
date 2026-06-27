using Avalonia;
using AvaloniaTemplate.Models.Enums.TemplatedControlTypes;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Controls;

public class ButtonFontStyleToolkit : BaseTemplatedControl
{
    #region Тип инструмента
    public static readonly StyledProperty<TemplateButtonFontStyleToolkitType> ToolkitTypeProperty =
        AvaloniaProperty.Register<ButtonClipboard, TemplateButtonFontStyleToolkitType>(nameof(ToolkitType));

    /// <summary>
    /// Тип инструмента
    /// </summary>
    public TemplateButtonFontStyleToolkitType ToolkitType
    {
        get => GetValue(ToolkitTypeProperty);
        set => SetValue(ToolkitTypeProperty, value);
    }
    #endregion

}