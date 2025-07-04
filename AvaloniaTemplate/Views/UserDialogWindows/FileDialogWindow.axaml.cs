using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaTemplate.Models.Enums.FileDialogOptionTypes;
using System.Collections.Generic;
using System.Threading;
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
    /// <returns></returns>
    public static string SelectFile(OpenFileDialogOptionType openFileDialogOption, IStorageProvider provider)
    {
        var SelectedFilePath = "";
        if (provider is not null)
        {
            var tcs = new TaskCompletionSource<IStorageFolder>();
            using var source = new CancellationTokenSource();
            var folderPath = provider.TryGetFolderFromPathAsync(openFileDialogOption.SuggestedStartLocation);
            folderPath.ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
            Dispatcher.UIThread.MainLoop(source.Token);

            if (folderPath is not null)
            {
                var dialog = provider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = openFileDialogOption.Title,
                    AllowMultiple = openFileDialogOption.AllowMultiple,
                    FileTypeFilter = [openFileDialogOption.FileTypeFilter],
                    SuggestedFileName = openFileDialogOption.SuggestedFileName,
                    SuggestedStartLocation = folderPath.Result
                });

                using var source1 = new CancellationTokenSource();
                var tcs1 = new TaskCompletionSource<List<IStorageFile>>();
                dialog.ContinueWith(t => source1.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
                Dispatcher.UIThread.MainLoop(source1.Token);

                if (dialog is not null)
                {
                    foreach (var item in dialog.Result)
                    {
                        SelectedFilePath += item.TryGetLocalPath();
                    }
                }
            }
        }
        return SelectedFilePath;
    }
    #endregion

    #region Открыть окно выбора пути
    /// <summary>
    /// Открыть окно выбора пути
    /// </summary>
    /// <returns></returns>
    public static string SelectFolder(OpenFileDialogOptionType openFileDialogOption, IStorageProvider provider = default)
    {
        var SelectedPath = "";
        if (provider is not null)
        {
            var tcs = new TaskCompletionSource<IStorageFolder>();
            using var source = new CancellationTokenSource();
            var folderPath = provider.TryGetFolderFromPathAsync(openFileDialogOption.SuggestedStartLocation);
            folderPath.ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
            Dispatcher.UIThread.MainLoop(source.Token);

            if (folderPath is not null)
            {
                var dialog = provider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                {
                    Title = openFileDialogOption.Title,
                    AllowMultiple = false,
                    SuggestedFileName = openFileDialogOption.SuggestedFileName,
                    SuggestedStartLocation = folderPath.Result
                });

                using var source1 = new CancellationTokenSource();
                var tcs1 = new TaskCompletionSource<List<IStorageFile>>();
                dialog.ContinueWith(t => source1.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
                Dispatcher.UIThread.MainLoop(source1.Token);

                if (dialog is not null)
                {
                    foreach (var item in dialog.Result)
                    {
                        SelectedPath += item.TryGetLocalPath();
                    }
                }
            }
        }
        return SelectedPath;

    }
    #endregion
}