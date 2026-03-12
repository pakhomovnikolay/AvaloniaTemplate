using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class DataGridHelper
    {
        public static readonly AttachedProperty<bool> AutoScrollToSelectedProperty =
            AvaloniaProperty.RegisterAttached<DataGridHelper, DataGrid, bool>("AutoScrollToSelected", false);

        static DataGridHelper()
        {
            AutoScrollToSelectedProperty.Changed.AddClassHandler<DataGrid>(OnAutoScrollChanged);
        }

        private static void OnAutoScrollChanged(DataGrid grid, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is true)
                grid.SelectionChanged += OnSelectionChanged;
            else
                grid.SelectionChanged -= OnSelectionChanged;
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not DataGrid grid || grid.SelectedItem is not { })
                return;

            if (!IsSelectedItemVisible(grid))
                ScrollToItem(grid);
        }

        private static void ScrollToItem(DataGrid grid)
        {
            Dispatcher.UIThread.Post(() =>
            {
                try
                {
                    // Пытаемся прокрутить к выбранному элементу
                    grid.ScrollIntoView(grid.SelectedItem, null);

                    // Дополнительно - пробуем прокрутить и к колонке
                    if (grid.Columns.Count > 0 && grid.CurrentColumn is { })
                        grid.ScrollIntoView(grid.SelectedItem, grid.CurrentColumn);
                }
                catch { /* Игнорируем */ }
            }, DispatcherPriority.Loaded); // Важен приоритет!
        }

        private static bool IsSelectedItemVisible(DataGrid dataGrid)
        {
            var result = false;
            if (dataGrid.SelectedItem is { })
            {
                var row = dataGrid.GetVisualDescendants()?
                    .OfType<DataGridRow>()?
                    .FirstOrDefault(row => row.DataContext == dataGrid.SelectedItem);

                if (row is { })
                {
                    var scrollViewer = dataGrid.FindDescendantOfType<ScrollViewer>();
                    if (scrollViewer is { }) {

                        var topLeft = row.TranslatePoint(new Point(0, 0), scrollViewer);
                        var bottomRight = row.TranslatePoint(new Point(row.Bounds.Width, row.Bounds.Height), scrollViewer);
                        if (topLeft is { } && bottomRight is { })
                        {
                            double viewportYStart = scrollViewer.Offset.Y;
                            double viewportYEnd = viewportYStart + scrollViewer.Viewport.Height;

                            bool isTopVisible = topLeft.Value.Y >= viewportYStart && topLeft.Value.Y <= viewportYEnd;
                            bool isBottomVisible = bottomRight.Value.Y >= viewportYStart && bottomRight.Value.Y <= viewportYEnd;

                            result = isTopVisible || isBottomVisible;
                        }
                    }
                }
            }

            return result;
        }
    }

    public static class VisualTreeExtensions
    {
        #region Поиск объекта по визульному дереву
        /// <summary>
        /// Поиск объекта по визульному дереву
        /// </summary>
        /// <typeparam name="T"> Тип искомого объекта </typeparam>
        /// <param name="visual"> Объект в котормо ведём поиск </param>
        /// <returns></returns>
        public static T FindDescendantOfType<T>(this Visual visual) where T : class
        {
            var result = default(T);
            if (visual is { })
            {
                foreach (var child in visual.GetVisualChildren())
                {
                    if (child is T found)
                    {
                        result = found;
                        break;
                    }
                    var descendant = child.FindDescendantOfType<T>();
                    if (descendant is { })
                    {
                        result = descendant;
                        break;
                    }
                }
            }
            return result;
        }
        #endregion
    }

}
