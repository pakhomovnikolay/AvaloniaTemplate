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

public class ButtonSetBackgroundStyle : TemplatedControl
{
    #region Команда
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ButtonPaste, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<ButtonCut, object?>(nameof(CommandParameter), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    #region Текущая заливка
    public static readonly StyledProperty<IBrush> CurrentBackgroundColorProperty =
        AvaloniaProperty.Register<ButtonPaste, IBrush>(nameof(CurrentBackgroundColor), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Текущая заливка
    /// </summary>
    public IBrush CurrentBackgroundColor
    {
        get => GetValue(CurrentBackgroundColorProperty);
        set => SetValue(CurrentBackgroundColorProperty, value);
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
        ColorHelper.CreateColorPalet(Command, frame, stackPanel);

        stackPanel.Children.Add(Helper.CreateLabel("Недавние цвета"));
        stackPanel.Children.Add(LayoutColorsRecent);

        stackPanel.Children.Add(new Separator() { Margin = new(0) });
        stackPanel.Children.Add(CreateButtonClearColor(Command, frame));
        stackPanel.Children.Add(new Separator() { Margin = new(0) });

        frame.Opened += (_, _) => CreateColorsRecent(Command, frame, LayoutColorsRecent);
        PopupContent = new() { Width = 230 };
        PopupContent.Children.Add(stackPanel); 
    }

    private static Button CreateButtonClearColor(ICommand command, Popup frame)
    {
        var button = new Button()
        {
            Background = Brushes.Transparent,
            Padding = new(5),
            CornerRadius = new(0),
            BorderThickness = new(0),
            VerticalAlignment = VerticalAlignment.Stretch,
            Content = "ОЧИСТИТЬ ЗАЛИВКУ",
            Command = command,
            CommandParameter = Brushes.Transparent
        };
        button.Classes.Add("highlightedBackground");

        button.Click += (_, _)
            => frame.Close();
        return button;
    }

    private static void CreateColorsRecent(ICommand command, Popup popupFrame, StackPanel LayoutColorsRecent)
    {
        //var stateService = App.GetService<IUIConnectorService>();
        //LayoutColorsRecent.Children.Clear();
        //foreach (var color in stateService.BackgroundColors)
        //    LayoutColorsRecent.Children.Add(ColorHelper.CreateButtonColor(command, popupFrame, color));
    }
}