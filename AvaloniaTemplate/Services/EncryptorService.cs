using AvaloniaTemplate.Infrastructures.Constants;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services
{
    public class EncryptorService : IEncryptorService
    {
        #region Асинхронное шифрование потока
        public async Task<DialogResult> EncryptStreamAsync(string sourcePath, string targetPath,
            string password = "",
            IProgress<double>? progress = null,
            CancellationToken cancel = default)
        {
            var cryptResult = DialogResult.Ok();
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException();

            byte[] key = new byte[EncryptFileFormat.KdfByteLength];
            try
            {
                byte[] salt = RandomNumberGenerator.GetBytes(EncryptFileFormat.SaltSize);
                byte[] baseNonce = RandomNumberGenerator.GetBytes(EncryptFileFormat.NonceSize);

                using var kdf = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    150_000,
                    HashAlgorithmName.SHA256);

                key = kdf.GetBytes(EncryptFileFormat.KdfByteLength);

                await using var input = File.OpenRead(sourcePath);
                await using var output = File.Create(targetPath);
                long total = input.Length;
                long processed = 0;

                // header
                await output.WriteAsync(EncryptFileFormat.MagicV3, cancel);
                await output.WriteAsync(salt, cancel);
                await output.WriteAsync(baseNonce, cancel);

                using var aes = new AesGcm(key, EncryptFileFormat.TagSize);
                byte[] buffer = new byte[EncryptFileFormat.ChunkSize];
                int read;
                uint counter = 0;

                while ((read = await input.ReadAsync(buffer, cancel)) > 0)
                {
                    var nonce = CreateNonce(baseNonce, counter++);
                    var ciphertext = new byte[read];
                    var tag = new byte[EncryptFileFormat.TagSize];

                    aes.Encrypt(nonce, buffer.AsSpan(0, read), ciphertext, tag);
                    var lengByte = new byte[EncryptFileFormat.LengthReadWriteByte];
                    BinaryPrimitives.WriteInt32LittleEndian(lengByte, read);

                    await output.WriteAsync(lengByte, cancel);
                    await output.WriteAsync(nonce, cancel);
                    await output.WriteAsync(tag, cancel);
                    await output.WriteAsync(ciphertext, cancel);

                    processed += read;
                    progress?.Report((double)processed / total);
                }
                progress?.Report(1);
                return cryptResult;
            }
            catch (OperationCanceledException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.OperationCanceledMessage, e);
                return cryptResult;
            }
            catch (CryptographicException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.CryptographicWriteMessage, e);
                return cryptResult;
            }
            catch (UnauthorizedAccessException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.UnauthorizedAccessMessage, e);
                return cryptResult;
            }
            catch (FileNotFoundException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.FileNotFoundMessage, e);
                return cryptResult;
            }
            finally
            {
                CryptographicOperations.ZeroMemory(key);
                if (!cryptResult.Success)
                    File.Delete(targetPath);
            }
        }
        #endregion

        #region Асинхронное дешифрование потока
        public async Task<DialogResult> DecryptStreamAsync(string sourcePath, string targetPath,
            string password = "",
            IProgress<double>? progress = null,
            CancellationToken cancel = default)
        {
            var cryptResult = DialogResult.Ok();
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException();

            byte[] key = new byte[EncryptFileFormat.KdfByteLength];

            try
            {
                await using var input = File.OpenRead(sourcePath);
                byte[] header = new byte[EncryptFileFormat.MagicV3.Length];
                await Helper.ReadExactlyAsync(input, header, cancel);
                if (!header.AsSpan().SequenceEqual(EncryptFileFormat.MagicV3))
                    throw new InvalidDataException();

                await using var output = File.Create(targetPath);
                byte[] salt = new byte[EncryptFileFormat.SaltSize];
                byte[] baseNonce = new byte[EncryptFileFormat.NonceSize];

                await Helper.ReadExactlyAsync(input, salt, cancel);
                await Helper.ReadExactlyAsync(input, baseNonce, cancel);

                using var kdf = new Rfc2898DeriveBytes(password, salt, 150_000, HashAlgorithmName.SHA256);
                key = kdf.GetBytes(EncryptFileFormat.KdfByteLength);

                using var aes = new AesGcm(key, EncryptFileFormat.TagSize);
                long total = input.Length;
                long processed = input.Position;

                while (input.Position < input.Length)
                {
                    cancel.ThrowIfCancellationRequested();

                    var lengByte = new byte[EncryptFileFormat.LengthReadWriteByte];
                    await Helper.ReadExactlyAsync(input, lengByte, cancel);
                    int chunkSize = BinaryPrimitives.ReadInt32LittleEndian(lengByte);
                    if (chunkSize <= 0 || chunkSize > EncryptFileFormat.ChunkSize)
                        throw new InvalidDataException();

                    var nonce = new byte[EncryptFileFormat.NonceSize];
                    var tag = new byte[EncryptFileFormat.TagSize];
                    var ciphertext = new byte[chunkSize];
                    var plaintext = new byte[chunkSize];

                    await Helper.ReadExactlyAsync(input, nonce, cancel);
                    await Helper.ReadExactlyAsync(input, tag, cancel);
                    await Helper.ReadExactlyAsync(input, ciphertext, cancel);

                    aes.Decrypt(nonce, ciphertext, tag, plaintext);

                    await output.WriteAsync(plaintext, cancel);

                    processed = input.Position;
                    progress?.Report((double)processed / total);
                }
                progress?.Report(1);
                return cryptResult;
            }
            catch (OperationCanceledException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.OperationCanceledMessage, e);
                return cryptResult;
            }
            catch (CryptographicException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.CryptographicReadMessage, e);
                return cryptResult;
            }
            catch (UnauthorizedAccessException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.UnauthorizedAccessMessage, e);
                return cryptResult;
            }
            catch (FileNotFoundException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.FileNotFoundMessage, e);
                return cryptResult;
            }
            catch (InvalidDataException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.InvalidDataMessage, e);
                return cryptResult;
            }
            catch (EndOfStreamException e)
            {
                cryptResult = DialogResult.Fail(EncryptFileFormat.EndOfStreamMessage, e);
                return cryptResult;
            }
            finally
            {
                CryptographicOperations.ZeroMemory(key);
                if (!cryptResult.Success)
                    File.Delete(targetPath);
            }
        }
        #endregion

        #region Создание кода для фрагмента шифрования
        /// <summary>
        /// Создание кода для фрагмента шифрования
        /// </summary>
        /// <param name="baseNonce"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        private static byte[] CreateNonce(byte[] baseNonce, uint counter)
        {
            byte[] nonce = new byte[baseNonce.Length];
            Buffer.BlockCopy(baseNonce, 0, nonce, 0, baseNonce.Length);

            // инкремент последних 4 байт
            BinaryPrimitives.WriteUInt32BigEndian(
                nonce.AsSpan(nonce.Length - EncryptFileFormat.LengthReadWriteByte),
                counter);

            return nonce;
        }
        #endregion
    }
}
