using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp42
{
    internal static class FileManager
    {
        // Переименование файла
        public static void RenameFile(string oldPath, string newName)
        {
            string newPath = Path.Combine(Path.GetDirectoryName(oldPath), newName);
            if (File.Exists(oldPath))
            {
                File.Move(oldPath, newPath);
                Console.WriteLine($"\nФайл переименован в {newName}");
            }
            else
            {
                Console.WriteLine("\nОшибка: файл не найден.");
            }
        }

        // Перемещение файла в новую директорию
        public static void MoveFile(string oldPath, string newPath)
        {
            try
            {
                if (File.Exists(oldPath))
                {
                    string target = Path.Combine(newPath, Path.GetFileName(oldPath));
                    if (File.Exists(target))
                    {
                        File.Delete(target); // Удаляем старый файл, если он уже существует в директории
                    }
                    File.Move(oldPath, target);
                    Console.WriteLine("\nФайл перемещен.");
                }
                else
                {
                    Console.WriteLine("\nОшибка: файл не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка при перемещении файла: {ex.Message}");
            }
        }

        // Удаление файла
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine("\nФайл удалён.");
            }
            else
            {
                Console.WriteLine("\nОшибка: файл не найден.");
            }
        }
    }
}
