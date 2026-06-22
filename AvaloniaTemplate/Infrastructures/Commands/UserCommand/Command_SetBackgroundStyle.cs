using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;
using System.Diagnostics;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommand
{
    public class Command_SetBackgroundStyle : Command
    {
        protected override bool CanExecute(object p)
            => p is not null;

        protected override void Execute(object p)
        {
            if (p is null || p is not IBrush color)
                return;

            var stateService = App.GetService<IGlobalStateService>();
            stateService.CurrentBackground = color;
            if (!stateService.BackgroundColors.Contains(Color.Parse(color.ToString())))
            {
                stateService.BackgroundColors.Insert(0, Color.Parse(color.ToString()));
                if (stateService.BackgroundColors.Count > 10)
                    stateService.BackgroundColors.RemoveAt(stateService.BackgroundColors.Count - 1);
            }
                



            //{
            //    stateService.CurrentBackground = button.Background;
            //    if (!stateService.BackgroundColors.Contains(color))
            //    {
            //        stateService.BackgroundColors.Insert(0, color);
            //        if (stateService.BackgroundColors.Count > 10)
            //            stateService.BackgroundColors.RemoveAt(stateService.BackgroundColors.Count - 1);
            //    }

            //}
            //;

            Debug.WriteLine($"Текущий цвет заливки: {color}");


            App.GetService<IUserDialogService>()?
                .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
