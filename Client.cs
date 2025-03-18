using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1
{
    internal class Client
    {
        static void Main()
        {
            while (true)
            {
                // Выводим меню с опциями для пользователя
                Console.WriteLine("1. Скачать новый файл");
                Console.WriteLine("2. Переименовать файл");
                Console.WriteLine("3. Переместить файл");
                Console.WriteLine("4. Удалить файл");
                Console.WriteLine("5. Выйти");
                Console.Write("Выбрать опцию: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Загрузка нового файла
                        Console.Clear();
                        Console.Write("Введите URL файла: ");
                        string URL = Console.ReadLine();
                        Console.Write("Введите путь для сохранения(с названием файла, например D:\\image.png): ");
                        string Path = Console.ReadLine().Trim('"');
                        string request = $"DOWNLOAD|{URL}|{Path}";
                        Console.Clear();
                        SendRequest(request);
                        break;
                    case "2": // Переименование файла
                        Console.Clear();
                        Console.Write("Введите текущий путь к файлу: ");
                        string OldPath = Console.ReadLine().Trim('"');
                        Console.Write("Введите новое имя файла: ");
                        string NewName = Console.ReadLine().Trim('"');
                        Console.Clear();
                        SendRequest($"RENAME|{OldPath}|{NewName}");
                        break;
                    case "3": // Перемещение файла
                        Console.Clear();
                        Console.Write("Введите текущий путь к файлу: ");
                        string currentPath = Console.ReadLine().Trim('"');
                        Console.Write("Введите новый путь: ");
                        string NewPath = Console.ReadLine().Trim('"');
                        Console.Clear();
                        SendRequest($"MOVE|{currentPath}|{NewPath}");
                        break;
                    case "4": // Удаление файла
                        Console.Clear();
                        Console.Write("Введите путь к файлу: ");
                        string DelFile = Console.ReadLine().Trim('"');
                        Console.Clear();
                        SendRequest($"DELETE|{DelFile}");
                        break;
                    case "5": // Выход из программы
                        Console.Write("Выход из программы.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        return;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }

        static void SendRequest(string request)
        {
            // Создаем TCP клиент для отправки запроса на сервер
            using (TcpClient client = new TcpClient("127.0.0.1", 5000))
            using (NetworkStream stream = client.GetStream())
            {
                // Преобразуем строку запроса в массив байтов
                byte[] data = Encoding.UTF8.GetBytes(request + "\n");

                // Отправляем данные на сервер
                stream.Write(data, 0, data.Length);
            }
        }
    }
}