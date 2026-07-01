using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Services.Interfaces;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class ButtonSetForegroundStyle : TemplatedControl
{
    #region Команда
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ButtonSetForegroundStyle, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Команда
    /// </summary>
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    #endregion

    #region Параметр для команды
    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<ButtonSetForegroundStyle, object?>(nameof(CommandParameter), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    #region Текущая цвет шрифта
    public static readonly StyledProperty<IBrush> CurrentForegroundColorProperty =
        AvaloniaProperty.Register<ButtonSetForegroundStyle, IBrush>(nameof(CurrentForegroundColor), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Текущая цвет шрифта
    /// </summary>
    public IBrush CurrentForegroundColor
    {
        get => GetValue(CurrentForegroundColorProperty);
        set => SetValue(CurrentForegroundColorProperty, value);
    }
    #endregion

    #region Источник данных раскрывающегося окна
    public static readonly StyledProperty<Panel> PopupContentProperty =
        AvaloniaProperty.Register<DualListSelector, Panel>(nameof(PopupContent), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных раскрывающегося окна
    /// </summary>
    public Panel PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }
    #endregion

    #region Окно раскрыто
    public static readonly StyledProperty<bool> IsPopupOpenProperty =
        AvaloniaProperty.Register<DualListSelector, bool>(nameof(IsPopupOpen), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Окно раскрыто
    /// </summary>
    public bool IsPopupOpen
    {
        get => GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var frame = e.NameScope.Find<Popup>("PART_Popup");
        var LayoutColorsRecent = Helper.CreateStackPanel();
        var stackPanel = Helper.CreateStackPanel(Orientation.Vertical, 5);

        stackPanel.Children.Add(Helper.CreateLabel("По умолчанию"));
        var buttonDefault = ColorHelper.CreateButtonColor(Command, frame, Color.Parse(Brushes.Black.ToString()));
        buttonDefault.HorizontalAlignment = HorizontalAlignment.Left;
        stackPanel.Children.Add(buttonDefault);

        ColorHelper.CreateColorPalet(Command, frame, stackPanel);

        stackPanel.Children.Add(Helper.CreateLabel("Недавние цвета"));
        stackPanel.Children.Add(LayoutColorsRecent);


        frame.Opened += (_, _) => CreateColorsRecent(Command, frame, LayoutColorsRecent);
        PopupContent = new() { Width = 230 };
        PopupContent.Children.Add(stackPanel);
    }

    private static void CreateColorsRecent(ICommand command, Popup popupFrame, StackPanel LayoutColorsRecent)
    {
        //var stateService = App.GetService<IUIConnectorService>();
        //LayoutColorsRecent.Children.Clear();
        //foreach (var color in stateService.ForegroundColors)
        //    LayoutColorsRecent.Children.Add(ColorHelper.CreateButtonColor(command, popupFrame, color));
    }
}