using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IEncryptorService
    {
        #region Синхронное шифрование
        /// <summary>
        /// Синхронное шифрование
        /// </summary>
        /// <param name="sourcePath">Путь к оригинальному файлу</param>
        /// <param name="destinationPath">Путь для сохранения зашифрованного файла</param>
        /// <param name="password">Пароль для файла</param>
        /// <param name="bufferLegth">Размер данных</param>
        void Encryptor(string sourcePath, string destinationPath, string password = "", int bufferLegth = 102400);
        #endregion

        #region Синхронная дешифровка
        /// <summary>
        /// Синхронная дешифровка
        /// </summary>
        /// <param name="sourcePath">Путь к оригинальному файлу</param>
        /// <param name="destinationPath">Путь к зашифрованному файлу</param>
        /// <param name="password">Пароль от файла</param>
        /// <param name="bufferLegth">Размер данных</param>
        /// <returns></returns>
        bool Decryption(string sourcePath, string destinationPath, string password = "", int bufferLegth = 102400);
        #endregion

        #region Асинхронное шифрование
        /// <summary>
        /// Асинхронное шифрование
        /// </summary>
        /// <param name="sourcePath">Путь к оригинальному файлу</param>
        /// <param name="destinationPath">Путь для сохранения зашифрованного файла</param>
        /// <param name="password">Пароль для файла</param>
        /// <param name="bufferLegth">Размер данных</param>
        /// <param name="progress">Статус шифрования</param>
        /// <param name="cancel">Признак отмены операции</param>
        /// <returns></returns>
        Task EncryptorAsync(string sourcePath, string destinationPath, string password = "", int bufferLegth = 102400,
            IProgress<double> progress = null, CancellationToken cancel = default);
        #endregion

        #region Асинхронное дешифровка
        /// <summary>
        /// Асинхронное дешифровка
        /// </summary>
        /// <param name="sourcePath">Путь к оригинальному файлу</param>
        /// <param name="destinationPath">Путь к зашифрованному файлу</param>
        /// <param name="password">Пароль от файла</param>
        /// <param name="bufferLegth">Размер данных</param>
        /// <param name="progress">Статус шифрования</param>
        /// <param name="cancel">Признак отмены операции</param>
        /// <returns></returns>
        Task<bool> DecryptionAsync(string sourcePath, string destinationPath, string password = "", int bufferLegth = 102400,
            IProgress<double> progress = null, CancellationToken cancel = default);
        #endregion
    }
}
