using Avalonia;
using Avalonia.Controls;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.Table.Base;
using AvaloniaTemplate.Models.Table.Base.Interfaces;
using AvaloniaTemplate.Models.Table.Model;
using System.Collections;
using System.Collections.Generic;
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
                        item.PositionX * Zoom,
                        item.PositionY * Zoom,
                        item.Width * Zoom,
                        item.Height * Zoom));
                }
            }

            //foreach (var child in Children)
            //{
            //    if (child.DataContext is IList<ISpreadsheetElement> list)
            //    {
            //        child.Arrange(new Rect(
            //            list.PositionX * Zoom,
            //            list.PositionY * Zoom,
            //            list.Width * Zoom,
            //            list.Height * Zoom));

            //        //                    if (list.DataContext is ISpreadsheetElement item) {

            //        //{

            //        //                    }

            //        //foreach (var item in list)
            //        //{
            //        //    if (item is ISpreadsheetElement element)
            //        //    {
            //        //        child.Arrange(new Rect(
            //        //            element.PositionX * Zoom,
            //        //            element.PositionY * Zoom,
            //        //            element.Width * Zoom,
            //        //            element.Height * Zoom));
            //        //    }
            //        //}
            //    }


            //    //if (child.DataContext is ISpreadsheetElement item)
            //    //{
            //    //    child.Arrange(new Rect(
            //    //        item.PositionX * Zoom,
            //    //        item.PositionY * Zoom,
            //    //        item.Width * Zoom,
            //    //        item.Height * Zoom));
            //    //}
            //}
            return finalSize;
        }
    }
}
