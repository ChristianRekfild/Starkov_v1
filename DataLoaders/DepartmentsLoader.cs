using Starkov_v1.CustomErrors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starkov_v1.DataLoaders
{
    public class DepartmentsLoader
    {
        private string _path;

        public DepartmentsLoader(string path)
        {
            _path = path;
        }

        public void Load()
        {
            using (StreamReader reader = new StreamReader(_path))
            {
                string line = reader.ReadLine();
                int lineNum = 1;
                while (line != null)
                {
                    try
                    {
                        if (lineNum == 1) continue;

                        // разбиваем строку достаём интересные нам данные
                        string[] arr = line.ToLower().Split('\t');
                        string name = arr[0].Trim();
                        name = name.Substring(0, 1).ToUpper() + name.Substring(1);

                        string parent = arr[1].Trim();
                        // string manager = arr[2].Trim(); // менеджера мы пока игнорируем, ибо нам понадобится загрузить их потом. В этом видимо и есть "прелесть" данного тестового, да? =)
                        string phone = arr[3].Trim();

                        using (StarkovContext context = new StarkovContext())
                        {
                            // Проверяем, есть ли такая провессия
                            if (context.Departments.Any(x => x.Name == name))
                            {
                                // такой элемент есть, мдём дальше
                                continue;
                            }

                            var newDepartment = new  Department();

                            newDepartment.Name = name.Substring(0, 1).ToUpper() + name.Substring(1);
                            newDepartment.Phone = phone;

                            // Если есть папа - нужно указать маленькому подразделению на большое и отдать в ручки
                            if (!string.IsNullOrWhiteSpace(parent))
                            {
                                parent = parent.Substring(0, 1).ToUpper() + parent.Substring(1);

                                var father = context.Departments.SingleOrDefault(x => x.Name == parent);
                                if (father != null)
                                    newDepartment.Parent = father;
                            }

                            // повторюсь, на загрузку менеджеров мы пока забиваем, ибо на данном этаме всё равно ничего не загрузим нормально. Нужно будет пройтись ещё раз
                            context.Add(newDepartment);
                            context.SaveChanges();
                        }


                    }
                    catch (Exception ex)
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

        /// <summary>
        /// Мы не загрузили менеджёров?
        /// А значит самое время сделать это, сказав "I'l be back!"
        /// </summary>
        public void LoadAgainToSaveManager()
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

                        // разбиваем строку достаём интересные нам данные
                        string[] arr = line.ToLower().Split('\t');
                        string name = arr[0].Trim();
                        name = name.Substring(0, 1).ToUpper() + name.Substring(1);

                        string manager = arr[2].Trim();
                        manager = myTI.ToTitleCase(manager);

                        using (StarkovContext context = new StarkovContext())
                        {
                            // Проверяем, есть ли такое подразделение
                            var existsDepartment = context.Departments.SingleOrDefault(x => x.Name == name);
                            if (existsDepartment is null)
                            {
                                throw new DepartmentNotFound($"При дозагрузке менеджеров подразделений не было найдено продразделение {name}");
                            }

                            // находим менеджера
                            var existingManager = context.Employees.SingleOrDefault(x => x.Fullname == manager);
                            if (existingManager is null)
                            {
                                throw new EmployeeNotFound($"Не найден сотрудник {manager}");
                            }
                            
                            existsDepartment.Manager = existingManager;

                            context.SaveChanges();
                        }


                    }
                    catch (Exception ex)
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
