using Starkov_v1.DataLoaders;
using System.Windows.Markup;
using System.Windows;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Starkov_v1.CustomErrors;
using Starkov_v1.ShowData;

namespace Starkov_v1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Вы хотите вывысти данные или загрузить?");

            string path = Directory.GetCurrentDirectory();
            string imortDataCatalog = path + "\\DataToImport";
            if (!Directory.Exists(imortDataCatalog))
                Directory.CreateDirectory(imortDataCatalog);

            Console.WriteLine("1 для вывода данных");
            Console.WriteLine("2 для загрузки");

            string answer = Console.ReadLine();
            switch (answer)
            {
                case "1": // Вывод данных
                    Visulizer visulizer = new Visulizer();
                    visulizer.ShowData();
                    break;
                case "2": // Загрузка
                    try
                    {
                        Loader loader = new Loader(imortDataCatalog);
                        loader.LoadData();
                    }
                    catch (RequiredFileNotFound ex)
                    {
                        Console.WriteLine("Не был найден один или несколько нужных файлов. Ниже будут указаны требуемые файлы. Так вы сможете найти отсутствующий");
                        Console.WriteLine("departments.tsv");
                        Console.WriteLine("employees.tsv");
                        Console.WriteLine("jobtitle.tsv");

                    } catch (Exception ex)
                    {
                        Console.WriteLine($"Упс! Что-то пошло не так! Смотри ошибку ниже:\n{ex.ToString()}");
                    } 

                    break;
                default:
                    Console.WriteLine("Введены некорректные данные");
                    return;
            }
                

            Console.ReadKey();
        }
    }
}