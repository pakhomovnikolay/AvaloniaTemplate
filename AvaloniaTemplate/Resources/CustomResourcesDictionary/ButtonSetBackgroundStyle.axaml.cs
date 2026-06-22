using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Commands.Base.Interfaces;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Services.Interfaces;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class ButtonSetBackgroundStyle : TemplatedControl
{
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

        var popupFrame = e.NameScope.Find<Popup>("PART_Popup");
        var stateService = App.GetService<IGlobalStateService>();
        var LayoutColorsRecent = Helper.CreateStackPanel();
        var command = App.GetService<ICommandProvider>()?.GetCommand("Command_SetBackgroundStyle");

        var currentBackgroundStyle = e.NameScope.Find<Border>("PART_CurrentBackgroundStyle");
        currentBackgroundStyle.Bind(BackgroundProperty, new Binding("CurrentBackground") { Source = stateService });
        stateService.CurrentBackground = Brushes.Yellow;

        var button = e.NameScope.Find<Button>("PART_SetBackgroundStyle");
        button.Command = command;
        button.Bind(Button.CommandParameterProperty, new Binding("CurrentBackground") { Source = stateService });

        var stackPanel = new StackPanel() { Spacing = 5 };
        Helper.CreateColorPalet(command, stateService, popupFrame, stackPanel, LayoutColorsRecent);

        stackPanel.Children.Add(new Separator() { Margin = new(0) });
        stackPanel.Children.Add(CreateButtonClearColor(command, stateService, popupFrame));
        stackPanel.Children.Add(new Separator() { Margin = new(0) });

        popupFrame.Opened += (_, _) => Helper.CreateColorsRecent(command, stateService, stackPanel, popupFrame, LayoutColorsRecent);
        PopupContent = new() { Width = 230 };
        PopupContent.Children.Add(stackPanel);
    }

    private static Button CreateButtonClearColor(ICommand command, IGlobalStateService stateService, Popup popupFrame)
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

        button.Click += (_, _) =>
        {
            stateService.CurrentBackground = button.Background;
            popupFrame.Close();
        };
        return button;
    }
}