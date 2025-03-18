using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp42
{
    internal static class DownloadManager
    {
        // Словарь для хранения активных загрузок.
        private static Dictionary<string, WebClient> ClientDownloads = new Dictionary<string, WebClient>();
        private static object LockObj = new object();

        // Запуск загрузки файла
        public static void StartDownload(string url, string path)
        {
            // Проверяем, существует ли файл с таким именем в указанной директории
            if (File.Exists(path))
            {
                Console.WriteLine("\nОшибка: Файл с таким именем уже существует.");
                return;
            }

            WebClient client = new WebClient();
            // Устанавливаем заголовок
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

            // Обработчик события изменения прогресса загрузки
            client.DownloadProgressChanged += (s, e) =>
            {
                // Переводим байты в мегабайты для удобства отображения
                double ByteReceived = e.BytesReceived / 1000000.0;
                double TotalReceive = e.TotalBytesToReceive / 1000000.0;
                // Выводим текущий прогресс загрузки в консоль
                Console.Write($"\rУстановлено: {ByteReceived:F2} MB / {TotalReceive:F2} MB ({e.ProgressPercentage}%)");
            };

            // Обработчик события завершения загрузки
            client.DownloadFileCompleted += (s, e) =>
            {
                // Если произошла ошибка, выводим сообщение об ошибке
                Console.WriteLine(e.Error != null ? $"\n{path}: Ошибка - {e.Error.Message}" : "\nСкачивание завершено");
                // Удаляем клиент загрузки из списка активных загрузок, чтобы освободить ресурсы
                lock (LockObj)
                {
                    ClientDownloads.Remove(path);
                }
            };

            // Блокируем доступ к словарю и добавляем новую загрузку
            lock (LockObj)
            {
                ClientDownloads[path] = client;
            }

            // Начинаем асинхронную загрузку файла по указанному URL
            client.DownloadFileAsync(new Uri(url), path);
            Console.WriteLine("Скачивание файла началось...");
        }
    }
}
