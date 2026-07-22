using Avalonia;
using Avalonia.Controls;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.SourceTable.Base;
using AvaloniaTemplate.Models.SourceTable.Base.Interfaces;
using AvaloniaTemplate.Models.SourceTable.Model;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Base
{
    public class SpreadsheetPanel : Panel
    {
        #region Масштаб
        public static readonly StyledProperty<double> ZoomProperty =
            AvaloniaProperty.Register<SpreadsheetPanel, double>(nameof(Zoom), defaultValue: 1.0);

        /// <summary>
        /// Масштаб
        /// </summary>
        public double Zoom
        {
            get => GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }
        #endregion

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var child in Children)
            {
                if (child.DataContext is ISpreadsheetElement item)
                {
                    child.Arrange(new Rect(
                        item.Geometry.PositionX * Zoom,
                        item.Geometry.PositionY * Zoom,
                        item.Geometry.Width * Zoom,
                        item.Geometry.Height * Zoom));
                }
            }
            return finalSize;
        }
    }
}
