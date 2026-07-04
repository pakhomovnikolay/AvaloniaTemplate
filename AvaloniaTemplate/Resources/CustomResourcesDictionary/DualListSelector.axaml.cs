using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Collections;
using System.Linq;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class DualListSelector : BaseTemplatedControl
{
    private bool MousePressed;
    private Point? StartPointerPressed;
    private DragDropDirectionType currentDropType;
    private Button buttonAdd;
    private Button buttonRemove;
    private ListBox sourceData;
    private ListBox targetData;
    private Func<object, object?> accessorFactory;
    private PointerPressedEventArgs pointerPressedEventArgs;
    static DualListSelector()
    {
        SourceItemsProperty.Changed.AddClassHandler<DualListSelector>((x, _) => x.RebuildSourceView());
        FilterSourceProperty.Changed.AddClassHandler<DualListSelector>((x, _) => x.RebuildSourceView());

        TargetItemsProperty.Changed.AddClassHandler<DualListSelector>((x, _) => x.RebuildTargetView());
        FilterTargetProperty.Changed.AddClassHandler<DualListSelector>((x, _) => x.RebuildTargetView());

        SourceSelectedItemProperty.Changed.AddClassHandler<DualListSelector>((x, _) => x.SourceSelectedItemChange());
        TargetSelectedItemProperty.Changed.AddClassHandler<DualListSelector>((x, _) => x.TargetSelectedItemChange());
    }

    #region public

    #region Шаблон представления данных
    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<DualListSelector, IDataTemplate?>(nameof(ItemTemplate));

    /// <summary>
    /// Шаблон представления данных
    /// </summary>
    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }
    #endregion

    #region Источник данных
    public static readonly StyledProperty<IList?> SourceItemsProperty =
        AvaloniaProperty.Register<DualListSelector, IList?>(nameof(SourceItems));

    /// <summary>
    /// Источник данных
    /// </summary>
    public IList? SourceItems
    {
        get => GetValue(SourceItemsProperty);
        set => SetValue(SourceItemsProperty, value);
    }

    #endregion

    #region Источник целевых данных
    public static readonly StyledProperty<IList?> TargetItemsProperty =
        AvaloniaProperty.Register<DualListSelector, IList?>(nameof(TargetItems));

    /// <summary>
    /// Источник целевых данных
    /// </summary>
    public IList? TargetItems
    {
        get => GetValue(TargetItemsProperty);
        set => SetValue(TargetItemsProperty, value);
    }
    #endregion

    #region Условие сортировки
    public static readonly StyledProperty<string?> SorterByProperty =
        AvaloniaProperty.Register<DualListSelector, string?>(nameof(SorterBy));

    /// <summary>
    /// Условие сортировки
    /// </summary>
    public string? SorterBy
    {
        get => GetValue(SorterByProperty);
        set => SetValue(SorterByProperty, value);
    }
    #endregion

    #region Style

    #region Цвет заднего фона содержимого
    public static readonly StyledProperty<IBrush?> BackgroundContentProperty =
        AvaloniaProperty.Register<DualListSelector, IBrush?>(
            nameof(BackgroundContent),
            defaultValue: Brushes.Orange);

    /// <summary>
    /// Цвет заднего фона содержимого
    /// </summary>
    public IBrush? BackgroundContent
    {
        get => GetValue(BackgroundContentProperty);
        set => SetValue(BackgroundContentProperty, value);
    }
    #endregion

    #region Цвет заднего фона панели управления
    public static readonly StyledProperty<IBrush?> BackgroundControlPanelProperty =
        AvaloniaProperty.Register<DualListSelector, IBrush?>(
            nameof(BackgroundControlPanel),
            defaultValue: Brushes.Orange);

    /// <summary>
    /// Цвет заднего фона панели управления
    /// </summary>
    public IBrush? BackgroundControlPanel
    {
        get => GetValue(BackgroundControlPanelProperty);
        set => SetValue(BackgroundControlPanelProperty, value);
    }
    #endregion

    #region Цвет заднего фона панели фильтра
    public static readonly StyledProperty<IBrush?> BackgroundPanelFilterProperty =
        AvaloniaProperty.Register<DualListSelector, IBrush?>(
            nameof(BackgroundPanelFilter),
            defaultValue: Brushes.CadetBlue);

    /// <summary>
    /// Цвет заднего фона панели фильтра
    /// </summary>
    public IBrush? BackgroundPanelFilter
    {
        get => GetValue(BackgroundPanelFilterProperty);
        set => SetValue(BackgroundPanelFilterProperty, value);
    }
    #endregion

    #region Цвет переднего фона содержимого
    public static readonly StyledProperty<IBrush?> ForegroundContentProperty =
        AvaloniaProperty.Register<DualListSelector, IBrush?>(
            nameof(ForegroundContent),
            defaultValue: Brushes.CornflowerBlue);

    /// <summary>
    /// Цвет переднего фона содержимого
    /// </summary>
    public IBrush? ForegroundContent
    {
        get => GetValue(ForegroundContentProperty);
        set => SetValue(ForegroundContentProperty, value);
    }
    #endregion

    #region Цвет переднего фона панели фильтра
    public static readonly StyledProperty<IBrush?> ForegroundPanelFilterProperty =
        AvaloniaProperty.Register<DualListSelector, IBrush?>(
            nameof(ForegroundPanelFilter),
            defaultValue: Brushes.White);

    /// <summary>
    /// Цвет переднего фона панели фильтра
    /// </summary>
    public IBrush? ForegroundPanelFilter
    {
        get => GetValue(ForegroundPanelFilterProperty);
        set => SetValue(ForegroundPanelFilterProperty, value);
    }
    #endregion

    #region Толщина границ содержимого
    public static readonly StyledProperty<Thickness> BoderThicknessContentProperty =
        AvaloniaProperty.Register<DualListSelector, Thickness>(
            nameof(BoderThicknessContent),
            defaultValue: new(2));

    /// <summary>
    /// Толщина границ содержимого
    /// </summary>
    public Thickness BoderThicknessContent
    {
        get => GetValue(BoderThicknessContentProperty);
        set => SetValue(BoderThicknessContentProperty, value);
    }
    #endregion

    #region Толщина границ панели управления
    public static readonly StyledProperty<Thickness> BoderThicknessControlPanelProperty =
        AvaloniaProperty.Register<DualListSelector, Thickness>(
            nameof(BoderThicknessControlPanel),
            defaultValue: new(1));

    /// <summary>
    /// Толщина границ панели управления
    /// </summary>
    public Thickness BoderThicknessControlPanel
    {
        get => GetValue(BoderThicknessControlPanelProperty);
        set => SetValue(BoderThicknessControlPanelProperty, value);
    }
    #endregion

    #region Цвет границы содержимого
    public static readonly StyledProperty<IBrush?> BorderBrushContentProperty =
        AvaloniaProperty.Register<DualListSelector, IBrush?>(
            nameof(BorderBrushContent),
            defaultValue: Brushes.Gray);

    /// <summary>
    /// Цвет границы содержимого
    /// </summary>
    public IBrush? BorderBrushContent
    {
        get => GetValue(BorderBrushContentProperty);
        set => SetValue(BorderBrushContentProperty, value);
    }
    #endregion

    #region Цвет границ панели управления
    public static readonly StyledProperty<IBrush?> BorderBrushControlPanelProperty =
        AvaloniaProperty.Register<DualListSelector, IBrush?>(
            nameof(BorderBrushControlPanel),
            defaultValue: Brushes.Gray);

    /// <summary>
    /// Цвет границ панели управления
    /// </summary>
    public IBrush? BorderBrushControlPanel
    {
        get => GetValue(BorderBrushControlPanelProperty);
        set => SetValue(BorderBrushControlPanelProperty, value);
    }
    #endregion

    #region Скругление границ содержимого
    public static readonly StyledProperty<CornerRadius> CornerRadiusContentProperty =
        AvaloniaProperty.Register<DualListSelector, CornerRadius>(
            nameof(CornerRadiusContent),
            defaultValue: new(3, 0, 0, 3));

    /// <summary>
    /// Скругление границ содержимого
    /// </summary>
    public CornerRadius CornerRadiusContent
    {
        get => GetValue(CornerRadiusContentProperty);
        set => SetValue(CornerRadiusContentProperty, value);
    }
    #endregion

    #region Скругление границ панели управления
    public static readonly StyledProperty<CornerRadius> CornerRadiusControlPanelProperty =
        AvaloniaProperty.Register<DualListSelector, CornerRadius>(
            nameof(CornerRadiusControlPanel),
            defaultValue: new(0));

    /// <summary>
    /// Скругление границ панели управления
    /// </summary>
    public CornerRadius CornerRadiusControlPanel
    {
        get => GetValue(CornerRadiusControlPanelProperty);
        set => SetValue(CornerRadiusControlPanelProperty, value);
    }
    #endregion

    #region Водяной знак панели поиска
    public static readonly StyledProperty<string?> WatermarkProperty =
        AvaloniaProperty.Register<DualListSelector, string?>(
            nameof(Watermark),
            defaultValue: "Поиск");

    /// <summary>
    /// Водяной знак панели поиска
    /// </summary>
    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }
    #endregion

    #endregion

    #endregion

    #region internal

    #region Видимые данные источника
    internal static readonly StyledProperty<IEnumerable?> SourceItemsViewProperty =
        AvaloniaProperty.Register<DualListSelector, IEnumerable?>(nameof(SourceItemsView));

    /// <summary>
    /// Видимые данные источника
    /// </summary>
    internal IEnumerable? SourceItemsView
    {
        get => GetValue(SourceItemsViewProperty);
        set => SetValue(SourceItemsViewProperty, value);
    }
    #endregion

    #region Видимые данные целевого источника
    internal static readonly StyledProperty<IEnumerable?> TargetItemsViewProperty =
        AvaloniaProperty.Register<DualListSelector, IEnumerable?>(nameof(TargetItemsView));

    /// <summary>
    /// Видимые данные целевого источника
    /// </summary>
    internal IEnumerable? TargetItemsView
    {
        get => GetValue(TargetItemsViewProperty);
        set => SetValue(TargetItemsViewProperty, value);
    }
    #endregion

    #region Текст для фильтрации источника данных
    internal static readonly StyledProperty<string?> FilterSourceProperty =
        AvaloniaProperty.Register<DualListSelector, string?>(nameof(FilterSource));

    /// <summary>
    /// Текст для фильтрации источника данных
    /// </summary>
    internal string? FilterSource
    {
        get => GetValue(FilterSourceProperty);
        set => SetValue(FilterSourceProperty, value);
    }
    #endregion

    #region Текст для фильтрации данных целевого источника
    internal static readonly StyledProperty<string?> FilterTargetProperty =
        AvaloniaProperty.Register<DualListSelector, string?>(nameof(FilterTarget));

    /// <summary>
    /// Текст для фильтрации данных целевого источника
    /// </summary>
    internal string? FilterTarget
    {
        get => GetValue(FilterTargetProperty);
        set => SetValue(FilterTargetProperty, value);
    }
    #endregion

    #region Скругление границ содержимого правой панели
    internal static readonly StyledProperty<CornerRadius> CornerRadiusContentRightProperty =
        AvaloniaProperty.Register<DualListSelector, CornerRadius>(
            nameof(CornerRadiusContentRight),
            defaultValue: new(3, 0, 0, 3));

    /// <summary>
    /// Скругление границ содержимого правой панели
    /// </summary>
    internal CornerRadius CornerRadiusContentRight
    {
        get => GetValue(CornerRadiusContentRightProperty);
        set => SetValue(CornerRadiusContentRightProperty, value);
    }
    #endregion

    #region Выбранные элементы источника данных
    internal static readonly StyledProperty<IList?> SourceSelectedItemsProperty =
        AvaloniaProperty.Register<DualListSelector, IList?>(nameof(SourceSelectedItems), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранные элементы источника данных
    /// </summary>
    internal IList? SourceSelectedItems
    {
        get => GetValue(SourceSelectedItemsProperty);
        set => SetValue(SourceSelectedItemsProperty, value);
    }
    #endregion

    #region Выбранные элементы целевого источника
    internal static readonly StyledProperty<IList?> TargetSelectedItemsProperty =
        AvaloniaProperty.Register<DualListSelector, IList?>(nameof(TargetSelectedItems), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранные элементы целевого источника
    /// </summary>
    internal IList? TargetSelectedItems
    {
        get => GetValue(TargetSelectedItemsProperty);
        set => SetValue(TargetSelectedItemsProperty, value);
    }
    #endregion

    #region Выбранный элемент источника данных
    internal static readonly StyledProperty<object> SourceSelectedItemProperty =
        AvaloniaProperty.Register<DualListSelector, object>(nameof(SourceSelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент источника данных
    /// </summary>
    internal object SourceSelectedItem
    {
        get => GetValue(SourceSelectedItemProperty);
        set => SetValue(SourceSelectedItemProperty, value);
    }
    #endregion

    #region Выбранный элемент целевого источника
    internal static readonly StyledProperty<object> TargetSelectedItemProperty =
        AvaloniaProperty.Register<DualListSelector, object>(nameof(TargetSelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент целевого источника
    /// </summary>
    internal object TargetSelectedItem
    {
        get => GetValue(TargetSelectedItemProperty);
        set => SetValue(TargetSelectedItemProperty, value);
    }
    #endregion

    #endregion

    #region private

    #region Обновить представление источника данных
    /// <summary>
    /// Обновить представление источника данных
    /// </summary>
    private void RebuildSourceView()
    {
        var view = SourceItems?.Cast<object>();
        if (!string.IsNullOrWhiteSpace(FilterSource))
            view = view?.Where(x =>
                x?.ToString()?.Contains(FilterSource,
                StringComparison.OrdinalIgnoreCase) == true);

        SourceItemsView = view?.OrderBy(x => accessorFactory?.Invoke(x))?.ToList();
    }
    #endregion

    #region Обновить представление целевого источника данных
    /// <summary>
    /// Обновить представление целевого источника данных
    /// </summary>
    private void RebuildTargetView()
    {
        var view = TargetItems?.Cast<object>();
        if (!string.IsNullOrWhiteSpace(FilterTarget))
            view = view?.Where(x =>
                x?.ToString()?.Contains(FilterTarget,
                StringComparison.OrdinalIgnoreCase) == true);

        TargetItemsView = view?.OrderBy(x => accessorFactory?.Invoke(x))?.ToList();
    }
    #endregion

    #region Обновить представление данных
    /// <summary>
    /// Обновить представление данных
    /// </summary>
    private void RebuildView()
    {
        RebuildSourceView();
        RebuildTargetView();
    }
    #endregion

    #region Переместить выбранные элементы в целевой источник
    /// <summary>
    /// Переместить выбранные элементы в целевой источник
    /// </summary>
    /// <param name="items"></param>
    private void MoveToTarget(IList items)
    {
        if (items is not { })
            return;

        var index = SourceItems.IndexOf(items[^1]);
        TargetSelectedItems?.Clear();
        for (int i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            TargetItems?.Add(item);
            TargetSelectedItems?.Add(item);
            SourceItems?.Remove(item);
        }
        RebuildView();
        SourceSelectedItem = Helper.GetSelectedElement<object>(index, SourceItems);
    }
    #endregion

    #region Переместить выбранные элементы источник данных
    /// <summary>
    /// Переместить выбранные элементы источник данных
    /// </summary>
    /// <param name="items"></param>
    private void MoveToSource(IList items)
    {
        if (items.Count <= 0)
            return;

        var index = TargetItems.IndexOf(items[^1]);
        SourceSelectedItems?.Clear();
        for (int i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            SourceItems?.Add(item);
            SourceSelectedItems?.Add(item);
            TargetItems?.Remove(item);
        }
        RebuildView();
        TargetSelectedItem = Helper.GetSelectedElement<object>(index, TargetItems);
    }
    #endregion

    #region Обработка нажатия ЛКМ на кнопку перемещения данных в целевой источник
    /// <summary>
    /// Обработка нажатия ЛКМ на кнопку перемещения данных в целевой источник
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleAddToSource(object sender, TappedEventArgs e)
    {
        if (TargetSelectedItem is not null)
            MoveToSource(TargetSelectedItems);
    }
    #endregion

    #region Обработка нажатия ЛКМ на кнопку перемещения данных в источник данных
    /// <summary>
    /// Обработка нажатия ЛКМ на кнопку перемещения данных в источник данных
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleAddToTarget(object sender, TappedEventArgs e)
    {
        if (SourceSelectedItem is not null)
            MoveToTarget(SourceSelectedItems);
    }
    #endregion

    #region Изменение выбранного элемента источника данных
    /// <summary>
    /// Изменение выбранного элемента источника данных
    /// </summary>
    private void SourceSelectedItemChange()
    {
        if (buttonAdd is not { })
            return;

        buttonAdd.IsEnabled = SourceSelectedItem is { };
    }
    #endregion

    #region Изменение выбранного элемента целевого источника данных
    /// <summary>
    /// Изменение выбранного элемента целевого источника данных
    /// </summary>
    private void TargetSelectedItemChange()
    {
        if (buttonRemove is not { })
            return;

        buttonRemove.IsEnabled = TargetSelectedItem is { };
    }
    #endregion

    #region DragDropSource

    #region Обработка двойного нажатия ЛКМ на элемент источника данных
    /// <summary>
    /// Обработка двойного нажатия ЛКМ на элемент источника данных
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SourceDoubleTappedHandler(object? sender, TappedEventArgs e)
    {
        if (SourceSelectedItem is not { })
            return;

        MoveToTarget(SourceSelectedItems);
    }
    #endregion

    #region Обработка нажатия ЛКМ по контейнеру элементов источника данных
    /// <summary>
    /// Обработка нажатия ЛКМ по контейнеру элементов источника данных
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SourcePointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        if (!e.Properties.IsLeftButtonPressed)
            return;

        var pos = e.GetPosition(sourceData);
        var obj = sourceData.InputHitTest(pos);
        if (obj is not ContentPresenter)
            return;

        pointerPressedEventArgs = e;
        StartPointerPressed = e.GetPosition(this);
        MousePressed = true;
    }
    #endregion

    #region Событие отпускания ЛКМ в контейнере элементов источника данных
    /// <summary>
    /// Событие отпускания ЛКМ в контейнере элементов источника данных
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SourcePointerReleasedHandler(object? sender, PointerReleasedEventArgs e)
    {
        currentDropType = DragDropDirectionType.Unknown;
        StartPointerPressed = null;
        MousePressed = false;
        pointerPressedEventArgs = null;
    }
    #endregion

    #region Событие перемещения мыши
    /// <summary>
    /// Событие перемещения мыши
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SourcePointerMovedHandler(object? sender, PointerEventArgs e)
    {
        if (!MousePressed || sourceData is not { } || sourceData.SelectedItem is null || StartPointerPressed is not { } || pointerPressedEventArgs is not { })
            return;

        const int dragThreshold = 5;
        var currentPosition = e.GetPosition(this);
        var delta = currentPosition - StartPointerPressed.Value;
        if (Math.Abs(delta.X) < dragThreshold && Math.Abs(delta.Y) < dragThreshold)
            return;

        currentDropType = DragDropDirectionType.ToTarget;
        StartPointerPressed = null;
        MousePressed = false;

        DragDrop.DoDragDropAsync(pointerPressedEventArgs, new DataTransfer(), DragDropEffects.Move);
    }
    #endregion

    #region Обработка события перетаскивания элемента
    /// <summary>
    /// Обработка события перетаскивания элемента
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SourceDragOver(object? sender, DragEventArgs e)
    {
        if (currentDropType == DragDropDirectionType.ToSource || currentDropType == DragDropDirectionType.BySource)
            e.DragEffects = DragDropEffects.Move;
        else
            e.DragEffects = DragDropEffects.None;
    }
    #endregion

    #region Обработка события завершения перетаскивания элемента
    /// <summary>
    /// Обработка события завершения перетаскивания элемента
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SourceDragLeaveHandler(object? sender, RoutedEventArgs e)
    {
        StartPointerPressed = null;
        MousePressed = false;
    }
    #endregion 

    #endregion

    #region DragDropTarget

    #region Обработка двойного нажатия ЛКМ на элемент целевого источника данных
    /// <summary>
    /// Обработка двойного нажатия ЛКМ на элемент целевого источника данных
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetDoubleTappedHandler(object? sender, TappedEventArgs e)
    {
        if (TargetSelectedItem is not { })
            return;

        MoveToSource(TargetSelectedItems);
    }
    #endregion

    #region Обработка нажатия ЛКМ по контейнеру элементов целевого источника данных
    /// <summary>
    /// Обработка нажатия ЛКМ по контейнеру элементов целевого источника данных
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetPointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        if (!e.Properties.IsLeftButtonPressed)
            return;

        var pos = e.GetPosition(targetData);
        var obj = targetData.InputHitTest(pos);
        if (obj is not ContentPresenter)
            return;

        StartPointerPressed = e.GetPosition(this);
        MousePressed = true;
    }
    #endregion

    #region Событие отпускания ЛКМ в контейнере элементов источника данных
    /// <summary>
    /// Событие отпускания ЛКМ в контейнере элементов источника данных
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetPointerReleasedHandler(object? sender, PointerReleasedEventArgs e)
    {
        currentDropType = DragDropDirectionType.Unknown;
        StartPointerPressed = null;
        MousePressed = false;
    }
    #endregion

    #region Событие перемещения мыши
    /// <summary>
    /// Событие перемещения мыши
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetPointerMovedHandler(object? sender, PointerEventArgs e)
    {
        if (!MousePressed || targetData is not { } || targetData.SelectedItem is null || StartPointerPressed is not { } || pointerPressedEventArgs is not { })
            return;

        const int dragThreshold = 5;
        var currentPosition = e.GetPosition(this);
        var delta = currentPosition - StartPointerPressed.Value;
        if (Math.Abs(delta.X) < dragThreshold && Math.Abs(delta.Y) < dragThreshold)
            return;

        currentDropType = DragDropDirectionType.ToSource;
        StartPointerPressed = null;
        MousePressed = false;

        DragDrop.DoDragDropAsync(pointerPressedEventArgs, new DataTransfer(), DragDropEffects.Move);
    }
    #endregion

    #region Обработка события перетаскивания элемента
    /// <summary>
    /// Обработка события перетаскивания элемента
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetDragOver(object? sender, DragEventArgs e)
    {
        if (currentDropType == DragDropDirectionType.ToTarget || currentDropType == DragDropDirectionType.ByTarget)
            e.DragEffects = DragDropEffects.Move;
        else
            e.DragEffects = DragDropEffects.None;
    }
    #endregion

    #region Обработка события завершения перетаскивания элемента
    /// <summary>
    /// Обработка события завершения перетаскивания элемента
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetDragLeaveHandler(object? sender, RoutedEventArgs e)
    {
        StartPointerPressed = null;
        MousePressed = false;
    }
    #endregion

    #endregion

    #region Обработка сброса перемещаемых элементов
    /// <summary>
    /// Обработка сброса перемещаемых элементов
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DropHandler(object? sender, DragEventArgs e)
    {
        if (sender is null || sourceData is not { } || targetData is not { })
            return;

        if (currentDropType == DragDropDirectionType.ToTarget && sourceData.SelectedItems is not null)
            MoveToTarget(sourceData.SelectedItems);
        else if (currentDropType == DragDropDirectionType.ToSource && targetData.SelectedItems is not null)
            MoveToSource(targetData.SelectedItems);

        currentDropType = DragDropDirectionType.Unknown;
        StartPointerPressed = null;
        MousePressed = false;
    }
    #endregion

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        CornerRadiusContentRight = new(
            CornerRadiusContent.TopRight,
            CornerRadiusContent.TopLeft,
            CornerRadiusContent.BottomLeft,
            CornerRadiusContent.BottomRight);

        SourceSelectedItems?.Add(Helper.GetSelectedElement<object>(0, SourceItems));
        TargetSelectedItems?.Add(Helper.GetSelectedElement<object>(0, TargetItems));

        buttonAdd = FindPartById<Button>(e, "PART_Add");
        buttonAdd.AddHandler(TappedEvent, HandleAddToTarget, handledEventsToo: true);

        buttonRemove = FindPartById<Button>(e, "PART_Remove");
        buttonRemove.AddHandler(TappedEvent, HandleAddToSource, handledEventsToo: true);

        sourceData = FindPartById<ListBox>(e, "PART_Source");
        targetData = FindPartById<ListBox>(e, "PART_Target");

        sourceData.AddHandler(DoubleTappedEvent, SourceDoubleTappedHandler, handledEventsToo: true);
        sourceData.AddHandler(PointerPressedEvent, SourcePointerPressedHandler, handledEventsToo: true);
        sourceData.AddHandler(PointerReleasedEvent, SourcePointerReleasedHandler, handledEventsToo: true);
        sourceData.AddHandler(PointerMovedEvent, SourcePointerMovedHandler, handledEventsToo: true);
        sourceData.AddHandler(DragDrop.DragOverEvent, SourceDragOver, handledEventsToo: true);
        sourceData.AddHandler(DragDrop.DragLeaveEvent, SourceDragLeaveHandler);

        targetData.AddHandler(DoubleTappedEvent, TargetDoubleTappedHandler, handledEventsToo: true);
        targetData.AddHandler(PointerPressedEvent, TargetPointerPressedHandler, handledEventsToo: true);
        targetData.AddHandler(PointerReleasedEvent, TargetPointerReleasedHandler, handledEventsToo: true);
        targetData.AddHandler(PointerMovedEvent, TargetPointerMovedHandler, handledEventsToo: true);
        targetData.AddHandler(DragDrop.DragOverEvent, TargetDragOver, handledEventsToo: true);
        targetData.AddHandler(DragDrop.DragLeaveEvent, TargetDragLeaveHandler);

        AddHandler(DragDrop.DropEvent, DropHandler, handledEventsToo: true);

        var itemType = SourceItems?.Cast<object>()?.FirstOrDefault()?.GetType();
        accessorFactory = App.GetService<IPropertyAccessorFactory>().Create(itemType, SorterBy);


        if (SourceSelectedItem is not { })
            SourceSelectedItemChange();

        if (TargetSelectedItem is not { })
            TargetSelectedItemChange();
    }
}