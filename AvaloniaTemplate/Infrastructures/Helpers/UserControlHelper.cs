using Avalonia;
using Avalonia.Controls;
using AvaloniaTemplate.Services;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class UserControlHelper
    {
        static UserControlHelper()
        {
            ViewportControlProperty.Changed.AddClassHandler<Control>(OnViewportChanged);
        }

        public static readonly AttachedProperty<bool> ViewportControlProperty =
            AvaloniaProperty.RegisterAttached<UserControlHelper, Control, bool>("ViewportControl");

        public static void SetViewportControl(Control control, bool value)
            => control.SetValue(ViewportControlProperty, value);

        public static bool GetViewportControl(Control control)
            => control.GetValue(ViewportControlProperty);

        private static void OnViewportChanged(Control control, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is true)
            {
                control.SizeChanged += (_, args) =>
                {
                    if (control.DataContext is not { }
                    || App.GetService<IUIConnectorService>() is not { } connectorService
                    || App.GetService<ITableGenerateFactory>() is not { } factory
                    ) return;

                    connectorService.WindowHeight = args.NewSize.Height;
                    connectorService.WindowWidth = args.NewSize.Width;
                    factory.UpdateViewport();

                    //TablesFactory { get; } = App.GetService<ITableGenerateFactory>();

                    //App.GetService<ITableGenerateFactory>()
                    //   .UpdateViewport(
                    //       args.NewSize.Width,
                    //       args.NewSize.Height);
                };
            }
        }
    }
}
