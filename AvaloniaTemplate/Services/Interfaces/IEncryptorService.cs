using AvaloniaTemplate.Models.Enums;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IEncryptorService
    {
        #region Асинхронное шифрование потока
        /// <summary>
        /// Асинхронное шифрование потока
        /// </summary>
        /// <param name="sourcePath">Полный путь к файлу</param>
        /// <param name="targetPath">Путь к дериктории для сохранения файла</param>
        /// <param name="password">Пароль? при необходимости</param>
        /// <param name="progress">Текущий прогресс шифрования</param>
        /// <param name="cancel">Ключ отмены опереции</param>
        /// <exception cref="OperationCanceledException">Отмена операции</exception>
        /// <exception cref="CryptographicException">Ошибка шифрования</exception>
        /// <exception cref="UnauthorizedAccessException">Не удалось получить доступ к файлу</exception>
        /// <exception cref="FileNotFoundException">Файл-источник для процесса шифрования не найден</exception>
        /// <returns>CryptResult</returns>
        Task<CryptResult> EncryptStreamAsync(string sourcePath, string targetPath,
            string password = "",
            IProgress<double>? progress = null,
            CancellationToken cancel = default);
        #endregion

        #region Асинхронное дешифрование потока
        /// <summary>
        /// Асинхронное дешифрование потока
        /// </summary>
        /// <param name="sourcePath">Полный путь к файлу</param>
        /// <param name="targetPath">Путь к дериктории для сохранения файла</param>
        /// <param name="password">Пароль? при необходимости</param>
        /// <param name="progress">Текущий прогресс шифрования</param>
        /// <param name="cancel">Ключ отмены опереции</param>
        /// <exception cref="CryptographicException">Ошибка шифрования</exception>
        /// <exception cref="OperationCanceledException">Отмена операции</exception>
        /// <exception cref="UnauthorizedAccessException">Не удалось получить доступ к файлу</exception>
        /// <exception cref="FileNotFoundException">Файл-источник для процесса шифрования не найден</exception>
        /// <exception cref="InvalidDataException">Неверный формат данных</exception>
        /// <exception cref="EndOfStreamException">Неожиданное окончание данных</exception>
        /// <returns>CryptResult</returns>
        Task<CryptResult> DecryptStreamAsync(string sourcePath, string targetPath,
            string password = "",
            IProgress<double>? progress = null,
            CancellationToken cancel = default);
        #endregion
    }
}
