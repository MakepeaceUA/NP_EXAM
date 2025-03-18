using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp42
{
    internal class Server
    {
        // Словарь для хранения клиентов, выполняющих загрузку файлов
        static Dictionary<string, WebClient> ClientDownloads = new Dictionary<string, WebClient>();
        static int MaxRequests = 3; // Максимальное количество запросов
        static int count = 0; // Текущий счетчик запросов
        static object LockObj = new object(); // Объект блокировки для синхронизации потоков
        static bool isBlocked = false; // Флаг блокировки сервера при перегрузке

        static void Main()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            Console.WriteLine("Сервер запущен...");

            while (true)
            {
                if (isBlocked)
                {
                    Console.WriteLine("Сервер временно заблокирован из-за перегрузки. Ожидание 2 минуты...");
                    Thread.Sleep(120000); // Ожидание 2 минуты при блокировке
                    lock (LockObj)
                    {
                        count = 0;
                        isBlocked = false;
                    }
                    Console.WriteLine("Сервер снова доступен.");
                }
                if (isBlocked)
                {
                    continue;
                }

                TcpClient client = server.AcceptTcpClient(); // Ожидание подключения клиента
                lock (LockObj)
                {
                    if (count >= MaxRequests)
                    {
                        isBlocked = true;
                        client.Close(); // Закрытие подключения при превышении лимита запросов
                        continue;
                    }
                    count++;
                }

                Thread thread = new Thread(() => RequestHandler.HandleClient(client));
                thread.Start(); // Запуск потока для обработки клиента
            }
        }
    }
}