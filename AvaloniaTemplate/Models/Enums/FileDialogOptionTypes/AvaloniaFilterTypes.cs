using Avalonia.Platform.Storage;

namespace AvaloniaTemplate.Models.Enums.FileDialogOptionTypes
{
    public class AvaloniaFilterTypes
    {
        public static FilePickerFileType ExcelFiles { get; } = new("Файлы Excel")
        {
            Patterns = ["*.xlsx", "*.xlsm"],
            AppleUniformTypeIdentifiers = ["public.xlsx"],
            MimeTypes = ["application/.xlsx"]

        };
    }
}
