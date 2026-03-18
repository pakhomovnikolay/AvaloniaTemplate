using AvaloniaTemplate.Services.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services
{
    public class LogService : ILogService
    {
        private static string dateTimePrev;

        #region Записать
        /// <summary>
        /// Записать
        /// </summary>
        /// <param name="log"></param>
        public void Write(string log)
        {
            var path = GetPathFile();
            try
            {
                using var sw = new StreamWriter(path, true, Encoding.UTF8);
                var message = $"[ {GetDateTimeNow()} ]\t{log}\n";
                sw.Write(message);
            }
            catch (Exception e)
            {
                path = GetPathFile(true);
                var message = $"[ {GetDateTimeNow()} ]\t{e.Message}\n";

                using var sw = new StreamWriter(path, true, Encoding.UTF8);
                sw.Write(message); // Exception Message

                message = $"[ {GetDateTimeNow()} ]\t{log}\n";
                sw.Write(message);
            }
        }
        #endregion

        #region Асинхронная запись лога
        /// <summary>
        /// Асинхронная запись лога
        /// </summary>
        /// <param name="log"></param>
        public async Task WriteAsync(string log)
        {
            var path = GetPathFile();
            try
            {
                await using var sw = new StreamWriter(path, true, Encoding.UTF8);
                var message = $"[ {GetDateTimeNow()} ]\t{log}\n";
                await sw.WriteAsync(message);
            }
            catch (Exception e)
            {
                path = GetPathFile(true);
                var message = $"[ {GetDateTimeNow()} ]\t{e.Message}\n";

                await using var sw = new StreamWriter(path, true, Encoding.UTF8);
                await sw.WriteAsync(message); // Exception Message

                message = $"[ {GetDateTimeNow()} ]\t{log}\n";
                await sw.WriteAsync(message);
            }
        }
        #endregion

        #region Получить путь
        /// <summary>
        /// Получить путь
        /// </summary>
        /// <returns></returns>
        private static string GetPathFile(bool AddTimeInNameFile = false)
        {
            var fileName = $"{DateTime.Now:yyyyMMdd}_Log.csv";
            if (AddTimeInNameFile)
                fileName = $"{DateTime.Now:yyyyMMdd_HH_mm_ss}_Log.csv";

            var result = Path.Combine(Directory.GetCurrentDirectory(), $"{App.AppName}Logs");
            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);

            return Path.Combine(result, fileName);
        }
        #endregion

        #region Получить метку времени
        /// <summary>
        /// Получить метку времени
        /// </summary>
        /// <returns></returns>
        private static string GetDateTimeNow(bool AddTimeInNameFile = false)
        {
            var format = "yyyy.MM.dd HH:mm:ss";
            var dateTimeNow = $"{DateTime.Now.ToString(format)}";
            if (dateTimeNow != dateTimePrev)
                dateTimePrev = dateTimeNow;
            else
                format = "yyyy.MM.dd HH:mm:ss:fff";

            return $"{DateTime.Now.ToString(format)}";
        }
        #endregion
    }
}
