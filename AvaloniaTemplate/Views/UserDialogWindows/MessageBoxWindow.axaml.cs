using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums.MessageTypes;
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
    private MessageBoxButtonType ButtonType { get; set; }
    private MessageBoxImageType ImageType { get; set; }
    private MessageBoxResultType ResultType { get; set; }
    #endregion

    #region Метод вызова окна выдачи сообщений
    /// <summary>
    /// Метод вызова окна выдачи сообщений
    /// </summary>
    /// <param name="option"></param>
    /// <param name="owner"></param>
    /// <returns></returns>
    public static async Task<MessageBoxResultType> ShowAsync(OpenMessageDialogOptionType option, Window owner)
    {
        var dialog = new MessageBox
        {
            Title = option.Title,
            Message = option.Message,
            ButtonType = option.ButtonType,
            ImageType = option.ImageType,
            ResultType = option.ResultType,
            Topmost = true,
            Icon = owner.Icon,
            CanMinimize = false
        };
        dialog.ButtonPanel = dialog.FindControl<StackPanel>("Buttons");
        await dialog.ShowDialog(owner);
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
                case MessageBoxButtonType.OKCancel:
                    AddButton("Ок", MessageBoxResultType.OK);
                    AddButton("Отмена", MessageBoxResultType.Cancel, true);
                    break;
                case MessageBoxButtonType.YesNoCancel:
                    AddButton("Да", MessageBoxResultType.Yes);
                    AddButton("Нет", MessageBoxResultType.No);
                    AddButton("Отмена", MessageBoxResultType.Cancel, true);
                    break;
                case MessageBoxButtonType.YesNo:
                    AddButton("Да", MessageBoxResultType.Yes);
                    AddButton("Нет", MessageBoxResultType.No, true);
                    break;
                default:
                    AddButton("Ок", MessageBoxResultType.OK, true);
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
    private void AddButton(string caption, MessageBoxResultType r, bool IsDefault = false)
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
            MessageBoxImageType.Question => Helper.GetResource<Image>("MessageBoxImageQuestion"),
            MessageBoxImageType.Information => Helper.GetResource<Image>("MessageBoxImageInformation"),
            MessageBoxImageType.Warning => Helper.GetResource<Image>("MessageBoxImageWarning"),
            MessageBoxImageType.Error => Helper.GetResource<Image>("MessageBoxImageError"),
            _ => Helper.GetResource<Image>("MessageBoxImageNone"),
        };

        if (icon.Source is not null)
            this.FindControl<Image>("Images").Source = icon.Source;
    }
    #endregion
}