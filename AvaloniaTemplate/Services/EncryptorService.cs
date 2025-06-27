using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services
{
    public class EncryptorService : IEncryptorService
    {
        #region Соль шифрования
        private static readonly byte[] __Salt =
        {
            0x26, 0xdc, 0xff, 0x00,
            0xad, 0xed, 0x7a, 0xee,
            0xc5, 0xfe, 0x07, 0xaf,
            0x4d, 0x08, 0x22, 0x3c
        };
        #endregion

        #region Шифрование
        private static ICryptoTransform GetEncryptor(byte[] salt = null)
        {
            using Aes aes = Aes.Create();
            aes.IV = MD5.HashData(salt ?? __Salt);
            aes.Key = SHA256.HashData(salt ?? __Salt);
            return aes.CreateEncryptor(aes.Key, aes.IV);
        }
        #endregion

        #region Расшифровка
        private static ICryptoTransform GetDecryption(byte[] salt = null)
        {
            using Aes aes = Aes.Create();
            aes.IV = MD5.HashData(salt ?? __Salt);
            aes.Key = SHA256.HashData(salt ?? __Salt);
            return aes.CreateDecryptor(aes.Key, aes.IV);
        }
        #endregion

        #region Синхронное шифрование
        public void Encryptor(string sourcePath, string destinationPath, string password, int bufferLength = 102400)
        {
            var encryptor = GetEncryptor();

            using var destination_encrypted = File.Create(destinationPath, bufferLength);
            using var destination = new CryptoStream(destination_encrypted, encryptor, CryptoStreamMode.Write);
            using var source = File.OpenRead(sourcePath);

            int readCount;
            var buffer = new byte[bufferLength];
            do
            {
                readCount = source.Read(buffer, 0, bufferLength);
                destination.Write(buffer, 0, readCount);
            } while (readCount > 0);

            destination.FlushFinalBlock();
        }
        #endregion

        #region Синхронная расшифровка
        public bool Decryption(string sourcePath, string destinationPath, string password, int bufferLength = 102400)
        {
            try
            {
                var decryption = GetDecryption();

                using var destination_decrypted = File.Create(destinationPath, bufferLength);
                using var destination = new CryptoStream(destination_decrypted, decryption, CryptoStreamMode.Write);
                using var source = File.OpenRead(sourcePath);

                int readCount;
                var buffer = new byte[bufferLength];
                do
                {
                    readCount = source.Read(buffer, 0, bufferLength);
                    destination.Write(buffer, 0, readCount);
                } while (readCount > 0);

                try
                {
                    destination.FlushFinalBlock();
                }
                catch (CryptographicException)
                {

                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Асинхронное шифрование
        public async Task EncryptorAsync(
            string sourcePath,
            string destinationPath,
            string password,
            int bufferLength = 102400,
            IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            if (!File.Exists(sourcePath)) throw new FileNotFoundException("Файл-источник для процесса шифрования не найден", sourcePath);
            if (bufferLength <= 0) throw new ArgumentOutOfRangeException(nameof(bufferLength), bufferLength, "Размер буфера чтения должен быть больше 0");

            cancel.ThrowIfCancellationRequested();

            var encryptor = GetEncryptor();

            try
            {
                await using var destination_encrypted = File.Create(destinationPath, bufferLength);
                await using var destination = new CryptoStream(destination_encrypted, encryptor, CryptoStreamMode.Write);
                await using var source = File.OpenRead(sourcePath);

                var file_length = source.Length;

                int readCount;
                var buffer = new byte[bufferLength];
                var last_percent = 0.0;
                do
                {
                    readCount = await source.ReadAsync(buffer.AsMemory(0, bufferLength), cancel).ConfigureAwait(false);
                    await destination.WriteAsync(buffer.AsMemory(0, readCount), cancel).ConfigureAwait(false);

                    var position = source.Position;
                    var percent = (double)position / file_length;

                    if ((percent - last_percent) >= 0.001)
                    {
                        progress?.Report(percent);
                        last_percent = percent;
                    }

                    if (cancel.IsCancellationRequested)
                        cancel.ThrowIfCancellationRequested();

                } while (readCount > 0);

                destination.FlushFinalBlock();

                progress?.Report(1);
            }
            catch (OperationCanceledException)
            {
                File.Delete(destinationPath);
                progress?.Report(0);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in EncryptorAsync:\r\n{0}", e);
                throw;
            }
        }
        #endregion

        #region Асинхронная расшифровка
        public async Task<bool> DecryptionAsync(
            string sourcePath,
            string destinationPath,
            string password,
            int bufferLength = 102400,
            IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            if (!File.Exists(sourcePath)) throw new FileNotFoundException("Файл-источник для процесса шифрования не найден", sourcePath);
            if (bufferLength <= 0) throw new ArgumentOutOfRangeException(nameof(bufferLength), bufferLength, "Размер буфера чтения должен быть больше 0");

            cancel.ThrowIfCancellationRequested();

            var decryption = GetDecryption();

            try
            {
                await using var destination_decrypted = File.Create(destinationPath, bufferLength);
                await using var destination = new CryptoStream(destination_decrypted, decryption, CryptoStreamMode.Write);
                await using var source = File.OpenRead(sourcePath);

                var file_length = source.Length;

                int readCount;
                var buffer = new byte[bufferLength];
                var last_percent = 0.0;
                do
                {
                    readCount = await source.ReadAsync(buffer.AsMemory(0, bufferLength), cancel).ConfigureAwait(false);
                    await destination.WriteAsync(buffer.AsMemory(0, readCount), cancel).ConfigureAwait(false);

                    var position = source.Position;
                    var percent = (double)position / file_length;

                    if ((percent - last_percent) >= 0.001)
                    {
                        progress?.Report(percent);
                        last_percent = percent;
                    }


                    cancel.ThrowIfCancellationRequested();
                } while (readCount > 0);

                try
                {
                    destination.FlushFinalBlock();
                }
                catch (CryptographicException)
                {
                    return false;
                }
                progress?.Report(1);
            }
            catch (OperationCanceledException)
            {
                File.Delete(destinationPath);
                progress?.Report(0);
                throw;
            }
            catch (Exception e)
            {

                Debug.WriteLine("Error in EncryptorAsync:\r\n{0}", e);
                throw;
            }

            return true;
        }
        #endregion
    }
}
