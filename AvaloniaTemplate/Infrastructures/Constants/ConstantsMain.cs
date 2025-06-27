namespace AvaloniaTemplate.Infrastructures.Constants
{
    public static class ConstantsMain
    {
        public const string __EncryptedConfigFileSuffix = ".configproject";
        public const string __EncryptedProjectFileSuffix = ".myprojectodb";
        public const string __SocketsExportFileSuffix = ".omx-export";
        public const string __XmlExportFileSuffix = ".xml";
        public const string __FilterSelectExcelWithMacros = "Книга Excel (*.xlsm*)|*.xlsm*";
        public const string __FilterSelectExcel = "Книга Excel (*.xlsm*)|*.xlsx*";
        public const string __FilterSelectProject = $"Файлы (*{__EncryptedProjectFileSuffix}*)|*{__EncryptedProjectFileSuffix}*";

        public const string __ConfigNameOriginal = $"Config{__XmlExportFileSuffix}";
        public const string __ConfigNameEncrypted = $"Config{__EncryptedConfigFileSuffix}";
    }
}
