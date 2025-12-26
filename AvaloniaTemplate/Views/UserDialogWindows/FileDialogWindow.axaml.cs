using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaTemplate.Models.Enums.FileDialogOptionTypes;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Views.UserDialogWindows;

public partial class FileDialogWindow : Window
{
    #region Конструктор
    /// <summary>
    /// Конструктор
    /// </summary>
    public FileDialogWindow() => InitializeComponent();
    #endregion

    #region Открыть окно выбора файла
    /// <summary>
    /// Открыть окно выбора файла
    /// </summary>
    /// <param name="options"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static async Task<string?> SelectFileAsync(OpenFileDialogOptionType options, IStorageProvider provider)
    {
        var result = "";
        var selectedPath = await provider.TryGetFolderFromPathAsync(options.SuggestedStartLocation);
        if (selectedPath is { })
        {
            var dialog = await provider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = options.Title,
                AllowMultiple = options.AllowMultiple,
                FileTypeFilter = [options.FileTypeFilter],
                SuggestedFileName = options.SuggestedFileName,
                SuggestedStartLocation = selectedPath,
                SuggestedFileType = options.FileTypeFilter
            });

            if (dialog is not null && dialog.Count > 0)
            {
                if (!options.AllowMultiple)
                    result = dialog[0].TryGetLocalPath();
                else
                    foreach (var path in dialog)
                        result += path.TryGetLocalPath();
            }
        }
        return result;
    }
    #endregion

    #region Открыть окно сохранения файла
    /// <summary>
    /// Открыть окно сохранения файла
    /// </summary>
    /// <param name="options"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static async Task<string?> SelectFileAsSaveAsync(OpenFileDialogOptionType options, IStorageProvider provider)
    {
        var result = "";
        var selectedPath = await provider.TryGetFolderFromPathAsync(options.SuggestedStartLocation);
        if (selectedPath is { })
        {
            var dialog = await provider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = options.Title,
                SuggestedFileName = options.SuggestedFileName,
                SuggestedStartLocation = selectedPath,
                DefaultExtension = options.FileTypeFilter.Name,
                FileTypeChoices = [options.FileTypeFilter],
                ShowOverwritePrompt = false,
                SuggestedFileType = options.FileTypeFilter
            });

            if (dialog is not null)
                result = dialog.TryGetLocalPath();
        }
        return result;
    }
    #endregion

    #region Открыть окно выбора пути
    /// <summary>
    /// Открыть окно выбора пути
    /// </summary>
    /// <param name="options"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static async Task<string?> SelectFolder(OpenFileDialogOptionType options, IStorageProvider provider = default)
    {
        var result = "";
        var selectedPath = await provider.TryGetFolderFromPathAsync(options.SuggestedStartLocation);
        if (selectedPath is { })
        {
            var dialog = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                AllowMultiple = options.AllowMultiple,
                SuggestedFileName = options.SuggestedFileName,
                SuggestedStartLocation = selectedPath,
                Title = options.Title,
            });

            if (dialog is not null && dialog.Count > 0)
            {
                if (!options.AllowMultiple)
                    result = dialog[0].TryGetLocalPath();
                else
                    foreach (var path in dialog)
                        result += path.TryGetLocalPath();
            }

        }
        return result;
    }
    #endregion
}