using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Commands.Base.Interfaces;
using AvaloniaTemplate.Models.Enums;
using System.Collections.Generic;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class ButtonSetGridStyle : TemplatedControl
{
    private Popup popupFrame;
    private readonly Dictionary<CurrentBorderStyleType, Control> bufferFrame = [];

    #region Источник данных
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<DualListSelector, object?>(nameof(Content), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных
    /// </summary>
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
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

    #region Источник данных
    public static readonly StyledProperty<bool> IsPopupOpenProperty =
        AvaloniaProperty.Register<DualListSelector, bool>(nameof(IsPopupOpen), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных
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
        popupFrame = e.NameScope.Find<Popup>("PART_Popup");

        var stackPanel = new StackPanel() { Spacing = 5 };
        
        stackPanel.Children.Add(CreateLabel("Границы"));
        stackPanel.Children.Add(CreateButtonGrid("Нижняя граница", CurrentBorderStyleType.Bottom));
        stackPanel.Children.Add(CreateButtonGrid("Верхняя граница", CurrentBorderStyleType.Top));
        stackPanel.Children.Add(CreateButtonGrid("Левая граница", CurrentBorderStyleType.Left));
        stackPanel.Children.Add(CreateButtonGrid("Правая граница", CurrentBorderStyleType.Right));

        stackPanel.Children.Add(new Separator() { Height = 2 });

        stackPanel.Children.Add(CreateButtonGrid("Нет границ", CurrentBorderStyleType.None));
        stackPanel.Children.Add(CreateButtonGrid("Все границы", CurrentBorderStyleType.All));
        stackPanel.Children.Add(CreateButtonGrid("Внешние границы", CurrentBorderStyleType.Outside));
        stackPanel.Children.Add(CreateButtonGrid("Толстые внешние границы", CurrentBorderStyleType.ThickOutside));
        PopupContent = new() { Width = 230 };
        PopupContent.Children.Add(stackPanel);

        if (bufferFrame.TryGetValue(CurrentBorderStyleType.Bottom, out var control))
        {
            Content = new Border()
            {
                Background = Brushes.White,
                Height = 25,
                Width = 25,
                Child = control
            };
        }
            
    }

    private static TextBlock CreateLabel(string label)
    {
        return new TextBlock()
        {
            Text = "Границы",
            FontWeight = FontWeight.Bold,
            Margin = new(5, 5, 0, 10),
        };
    }

    private Button CreateButtonGrid(string label, CurrentBorderStyleType borderStyle)
    {
        var width = 20;
        var height = 20;
        var desc = new TextBlock()
        {
            Text = label,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new(10, 0, 0, 0)
        };
        var border = new Border()
        {
            Background = Brushes.White,
            Height = height,
            Width = width,
            Child = CreateButtonGrid(width, height, borderStyle)

        };
        var grid = new Grid()
        {
            ColumnDefinitions = [
                new ColumnDefinition(25, GridUnitType.Pixel),
                new ColumnDefinition(25, GridUnitType.Star)
                ]
        };
        Grid.SetColumn(desc, 1);
        Grid.SetColumn(border, 0);
        grid.Children.Add(desc);
        grid.Children.Add(border);

        var button = new Button()
        {
            Content = grid,
            Padding = new(5),
            BorderThickness = new(0),
            CornerRadius = new(0),
            HorizontalContentAlignment = HorizontalAlignment.Left,
            Background = Brushes.Transparent,
            Command = App.GetService<ICommandProvider>()?.GetCommand("Command_ChangeStyleGrid"),
            CommandParameter = borderStyle
        };

        //?.Execute(borderStyle)
        button.Click += (_, _) =>
        {
            Content = CreateButtonGrid(width, height, borderStyle);
            popupFrame?.Close();
        };
        button.Classes.Add("highlightedBackground");
        return button;
    }

    private Border CreateButtonGrid(double width, double height, CurrentBorderStyleType borderStyle)
    {
        var border = new Border()
        {
            Background = Brushes.White,
            Height = height,
            Width = width,
            Child = borderStyle switch
            {
                CurrentBorderStyleType.Bottom => GridStyleType.CreateGrid(width, height, Bottom: BorderStyleType.Normal),
                CurrentBorderStyleType.Top => GridStyleType.CreateGrid(width, height, Top: BorderStyleType.Normal),
                CurrentBorderStyleType.Left => GridStyleType.CreateGrid(width, height, Left: BorderStyleType.Normal),
                CurrentBorderStyleType.Right => GridStyleType.CreateGrid(width, height, Right: BorderStyleType.Normal),
                CurrentBorderStyleType.All => GridStyleType.CreateGrid(width, height,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal),

                CurrentBorderStyleType.Outside => GridStyleType.CreateGrid(width, height,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal
                    ),

                CurrentBorderStyleType.ThickOutside => GridStyleType.CreateGrid(width - 1, height - 1,
                    BorderStyleType.Thick,
                    BorderStyleType.Thick,
                    BorderStyleType.Thick,
                    BorderStyleType.Thick
                    ),

                _ => GridStyleType.CreateGrid(width, height)
            }
        };

        if (!bufferFrame.ContainsKey(borderStyle))
            bufferFrame[borderStyle] = borderStyle switch
            {
                CurrentBorderStyleType.Bottom => GridStyleType.CreateGrid(width, height, Bottom: BorderStyleType.Normal),
                CurrentBorderStyleType.Top => GridStyleType.CreateGrid(width, height, Top: BorderStyleType.Normal),
                CurrentBorderStyleType.Left => GridStyleType.CreateGrid(width, height, Left: BorderStyleType.Normal),
                CurrentBorderStyleType.Right => GridStyleType.CreateGrid(width, height, Right: BorderStyleType.Normal),
                CurrentBorderStyleType.All => GridStyleType.CreateGrid(width, height,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal),

                CurrentBorderStyleType.Outside => GridStyleType.CreateGrid(width, height,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal,
                    BorderStyleType.Normal
                    ),

                CurrentBorderStyleType.ThickOutside => GridStyleType.CreateGrid(width - 1, height - 1,
                    BorderStyleType.Thick,
                    BorderStyleType.Thick,
                    BorderStyleType.Thick,
                    BorderStyleType.Thick
                    ),

                _ => GridStyleType.CreateGrid(width, height)
            };

        return border;
    }
}