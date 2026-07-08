using Avalonia;
using Avalonia.Controls.Primitives;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System.Collections.ObjectModel;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterColumns : BaseTemplatedControl
{
    #region Источник данных
    public static readonly StyledProperty<ObservableCollection<ModelColumn>> ItemsSourceProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, ObservableCollection<ModelColumn>>(nameof(ItemsSource));

    /// <summary>
    /// Источник данных
    /// </summary>
    public ObservableCollection<ModelColumn> ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion
}