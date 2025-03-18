using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp42
{
    internal static class RequestHandler
    {
        // Обработка запросов от клиента
        public static void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string request = reader.ReadLine(); // Читаем запрос от клиента
                if (request != null)
                {
                    string[] parts = request.Split('|'); // Разбиваем строку по разделителю '|'
                    switch (parts[0])
                    {
                        case "DOWNLOAD":
                            DownloadManager.StartDownload(parts[1], parts[2]);
                            break;
                        case "RENAME":
                            FileManager.RenameFile(parts[1], parts[2]);
                            break;
                        case "MOVE":
                            FileManager.MoveFile(parts[1], parts[2]);
                            break;
                        case "DELETE":
                            FileManager.DeleteFile(parts[1]);
                            break;
                        default:
                            Console.WriteLine("\nОшибка: неизвестная команда.");
                            break;
                    }
                }
            }
        }
    }
}
