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

public class ButtonSetForegroundStyle : TemplatedControl
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
        var command = App.GetService<ICommandProvider>()?.GetCommand("Command_SetForegroundStyle");

        var currentBackgroundStyle = e.NameScope.Find<Border>("PART_CurrentBackgroundStyle");
        currentBackgroundStyle.Bind(BackgroundProperty, new Binding("CurrentForeground") { Source = stateService });
        stateService.CurrentForeground = Brushes.Red;

        var button = e.NameScope.Find<Button>("PART_SetBackgroundStyle");
        button.Command = command;
        button.Bind(Button.CommandParameterProperty, new Binding("CurrentForeground") { Source = stateService });

        var stackPanel = Helper.CreateStackPanel(Orientation.Vertical, 5);
        stackPanel.Margin = new(0, 0, 0, 5);

        stackPanel.Children.Add(Helper.CreateLabel("По умолчанию"));
        var buttonDefault = Helper.CreateButtonColor(command, popupFrame, Color.Parse(Brushes.Black.ToString()));
        buttonDefault.HorizontalAlignment = HorizontalAlignment.Left;
        stackPanel.Children.Add(buttonDefault);

        Helper.CreateColorPalet(command, popupFrame, stackPanel, LayoutColorsRecent);

        stackPanel.Children.Add(Helper.CreateLabel("Недавние цвета"));
        stackPanel.Children.Add(LayoutColorsRecent);
        popupFrame.Opened += (_, _) => CreateColorsRecent(command, stateService, popupFrame, LayoutColorsRecent);

        PopupContent = new() { Width = 230 };
        PopupContent.Children.Add(stackPanel);
    }

    private static void CreateColorsRecent(ICommand command, IGlobalStateService stateService, Popup popupFrame, StackPanel LayoutColorsRecent)
    {
        LayoutColorsRecent.Children.Clear();
        foreach (var color in stateService.ForegroundColors)
            LayoutColorsRecent.Children.Add(Helper.CreateButtonColor(command, popupFrame, color));
    }
}