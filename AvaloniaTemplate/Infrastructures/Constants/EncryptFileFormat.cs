namespace AvaloniaTemplate.Infrastructures.Constants
{
    public class EncryptFileFormat
    {
        public const int SaltSize = 16;
        public const int NonceSize = 12;
        public const int KdfByteLength = 32;
        public const int TagSize = 16;
        public const int ChunkSize = 1024 * 1024; // 1 MB
        public const int LengthReadWriteByte = 4; // 1 MB

        public const string OperationCanceledMessage = "Операция отменена";
        public const string CryptographicReadMessage = "Неверный пароль или файл повреждён";
        public const string CryptographicWriteMessage = "Ошибка шифрования";
        public const string UnauthorizedAccessMessage = "Не удалось получить доступ к файлу";
        public const string FileNotFoundMessage = "Файл-источник для процесса шифрования не найден";
        public const string InvalidDataMessage = "Неверный формат данных";
        public const string EndOfStreamMessage = "Неожиданное окончание данных";

        public static byte[] MagicV1 => "ENC1"u8.ToArray();
        public static byte[] MagicV2 => "ENC2"u8.ToArray();
        public static byte[] MagicV3 => "KTS1"u8.ToArray();
        public static byte[] MagicV4 => "SPK1"u8.ToArray();
    }
}