using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Starkov_v1.DataLoaders
{
    public class JobTitleLoader
    {
        private string _path;

        public JobTitleLoader(string path)
        {
            _path = path;
        }

        public void Load()
        {
            TextInfo myTI = new CultureInfo("ru-RU", false).TextInfo;

            using (StreamReader reader = new StreamReader(_path))
            {
                string line = reader.ReadLine();
                int lineNum = 1;
                while (line != null)
                {
                    try
                    {
                        if (lineNum == 1) continue;

                        line = myTI.ToTitleCase(line.ToLower());

                        using (StarkovContext context = new StarkovContext())
                        {
                            // Проверяем, есть ли такая профессия
                            if (context.JobTitles.Any(x => x.Name == line))
                            {
                                // такой элемент есть, идём дальше
                                continue;
                            }

                            var newJobTitle = new JobTitle();
                            newJobTitle.Name = line;

                            context.Add(newJobTitle);
                            context.SaveChanges();
                        }


                    } catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.ToString()}");
                    }
                    finally
                    {
                        line = reader.ReadLine();
                        lineNum++;
                    }

                }
            }
        }


    }
}
