using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.SourceTable.Base;
using AvaloniaTemplate.Models.SourceTable.Model;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.Models.SourceTable
{
    public class TableRow : ObservableObject
    {
        #region Конструктор класса
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TableRow() { }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TableRow(ModelRow row)
        {
            SourceRow = row;
            CellStyle = new()
            {
                Background = Helper.GetColor(row.CellStyle.Background),
                Foreground = Helper.GetColor(row.CellStyle.Foreground),
                BorderBrush = Helper.GetColor(row.CellStyle.BorderBrush),
                BorderThickness = Helper.GetThickness(
                    row.CellStyle.BorderLeftStyle,
                    row.CellStyle.BorderTopStyle,
                    row.CellStyle.BorderRightStyle,
                    row.CellStyle.BorderBottomStyle),
                CurrentFontWeight = row.CellStyle.IsBold ? FontWeight.Bold : FontWeight.Normal,
                CurrentFontStyle = row.CellStyle.IsItalic ? FontStyle.Italic : FontStyle.Normal,
                IsUnderline = row.CellStyle.IsUnderline,
                IsWrap = row.CellStyle.IsWrap,
                FontFamily = Helper.GetFontFamily(row.CellStyle.FontFamily),
                FontSize = row.CellStyle.FontSize,
                HorizontalContentAlignment = Helper.GetHorizontalAlignment(row.CellStyle.HorizontalContentAlignment),
                VerticalContentAlignment = Helper.GetVerticalAlignment(row.CellStyle.VerticalContentAlignment),
            };
        }
        #endregion

        #region Источник данных
        private ModelRow sourceRow;
        /// <summary>
        /// Источник данных
        /// </summary>
        public ModelRow SourceRow
        {
            get => sourceRow;
            set => SetProperty(ref sourceRow, value);
        }
        #endregion

        #region Стиль ячейки
        private TableCellStyle cellStyle;
        /// <summary>
        /// Стиль ячейки
        /// </summary>
        public TableCellStyle CellStyle
        {
            get => cellStyle;
            set => SetProperty(ref cellStyle, value);
        }
        #endregion
    }
}
