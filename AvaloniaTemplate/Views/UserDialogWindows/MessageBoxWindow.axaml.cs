using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using AvaloniaTemplate.Models.Enums.MessageTypes;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Views.UserDialogWindows;

public partial class MessageBox : Window
{
    #region Конструктор
    /// <summary>
    /// Контструктор
    /// </summary>
    public MessageBox() => InitializeComponent();
    #endregion

    #region Поля класса
    private string Message { set => this.FindControl<TextBlock>("Contents").Text = value; }
    private MessageBoxButton ButtonType { get; set; }
    private MessageBoxImage ImageType { get; set; }
    private MessageBoxResult ResultType { get; set; }
    #endregion

    #region Метод вызова окна выдачи сообщений
    /// <summary>
    /// Метод вызова окна выдачи сообщений
    /// </summary>
    /// <param name="title">Заголовок окна</param>
    /// <param name="message">Сообщение для пользователя</param>
    /// <param name="messageBoxButtonType">Конфигурация кнопок управления</param>
    /// <param name="messageBoxImagType">Конфигурация изображения</param>
    /// <param name="messageBoxResultType">Конфигурация результата диалога</param>
    /// <returns></returns>
    public static MessageBoxResult Show(string title, string message, MessageBoxButton messageBoxButtonType, MessageBoxImage messageBoxImagType, MessageBoxResult messageBoxResultType, Window ownerWindow)
    {
        var dialog = new MessageBox
        {
            Title = title,
            Message = message,
            ButtonType = messageBoxButtonType,
            ImageType = messageBoxImagType,
            ResultType = messageBoxResultType,
            Topmost = true,
        };

        dialog.ButtonPanel = dialog.FindControl<StackPanel>("Buttons");
        var tcs = new TaskCompletionSource<MessageBoxResult>();

        dialog.Closed += delegate { tcs.TrySetResult(dialog.ResultType); };

        using var source = new CancellationTokenSource();
        dialog.ShowDialog(ownerWindow).ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
        Dispatcher.UIThread.MainLoop(source.Token);
        return dialog.ResultType;

    }
    #endregion

    #region Инициализация StackPanel с кнопками управления
    private StackPanel buttonPanel;
    /// <summary>
    /// Инициализация StackPanel с кнопками управления
    /// </summary>
    private StackPanel ButtonPanel
    {
        get => buttonPanel;
        set
        {
            buttonPanel = value;
            LoadImageSourceFromResource();
            switch (ButtonType)
            {
                case MessageBoxButton.OKCancel:
                    AddButton("Ок", MessageBoxResult.OK);
                    AddButton("Отмена", MessageBoxResult.Cancel, true);
                    break;
                case MessageBoxButton.YesNoCancel:
                    AddButton("Да", MessageBoxResult.Yes);
                    AddButton("Нет", MessageBoxResult.No);
                    AddButton("Отмена", MessageBoxResult.Cancel, true);
                    break;
                case MessageBoxButton.YesNo:
                    AddButton("Да", MessageBoxResult.Yes);
                    AddButton("Нет", MessageBoxResult.No, true);
                    break;
                default:
                    AddButton("Ок", MessageBoxResult.OK, true);
                    break;
            }
        }
    }
    #endregion

    #region Добавление кнопок управления в StackPanel
    /// <summary>
    /// Добавление кнопок управления в StackPanel
    /// </summary>
    /// <param name="caption"></param>
    /// <param name="r"></param>
    /// <param name="IsDefault"></param>
    private void AddButton(string caption, MessageBoxResult r, bool IsDefault = false)
    {
        var btn = new Button { Content = caption };
        btn.Click += (_, _) =>
        {
            this.ResultType = r;
            this.Close();
        };

        this.ButtonPanel.Children.Add(btn);
        if (IsDefault)
        {
            btn.HotKey = new KeyGesture(Key.Escape);
            this.ResultType = r;
        }

    }
    #endregion

    #region Загрузка изображения из ресурсов
    /// <summary>
    /// Загрузка изображения из ресурсов
    /// </summary>
    private void LoadImageSourceFromResource()
    {
        var icon = ImageType switch
        {
            MessageBoxImage.Question => (Image)Application.Current.FindResource("MessageBoxImageQuestion"),
            MessageBoxImage.Information => (Image)Application.Current.FindResource("MessageBoxImageInformation"),
            MessageBoxImage.Warning => (Image)Application.Current.FindResource("MessageBoxImageWarning"),
            MessageBoxImage.Error => (Image)Application.Current.FindResource("MessageBoxImageError"),
            _ => (Image)Application.Current.FindResource("MessageBoxImageNone"),
        };

        if (icon.Source is not null)
            this.FindControl<Image>("Images").Source = icon.Source;
    }
    #endregion
}