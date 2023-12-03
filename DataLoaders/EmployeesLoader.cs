using Starkov_v1.CustomErrors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Starkov_v1.DataLoaders
{
    public class EmployeesLoader
    {
        private string _path;

        public EmployeesLoader(string path) 
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

                        using (StarkovContext context = new StarkovContext())
                        {
                            string[] arr = line.Split('\t');

                            string department = arr[0].Trim().ToLower();
                            department = department.Substring(0, 1).ToUpper() + department.Substring(1);

                            var existingDepartment = context.Departments.SingleOrDefault(x => x.Name == department);
                            if (existingDepartment is null)
                            {
                                throw new DepartmentNotFound($"Не найдено подразделение {department}");
                            }

                            string fullName = arr[1].Trim().ToLower();
                            fullName = myTI.ToTitleCase(fullName); // Делаем ФИО с большой буквы, а то пользователи потом обидятся на нас...

                            string login = arr[2].Trim(); 
                            string password = arr[3]; // никакого trimm. Это пароль!

                            string jobTitle = arr[4].Trim().ToLower();
                            jobTitle = myTI.ToTitleCase(jobTitle);

                            var existingjobTitle = context.JobTitles.SingleOrDefault(x => x.Name == jobTitle);
                            if (existingjobTitle is null)
                            {
                                throw new JobTitleNotFound($"Не найдена должность {jobTitle}");
                            }


                            // Проверяем, есть ли такая провессия
                            if (context.Employees.Any(x => x.Fullname == fullName && x.Login == login 
                                && x.Password == password))
                            {
                                // такой элемент есть, идём дальше
                                continue;
                            }

                            Employee newEmployee = new Employee();
                            newEmployee.Fullname = fullName;
                            newEmployee.Login = login;
                            newEmployee.Password = password;
                            newEmployee.Departments.Add(existingDepartment); // EF Core немного криво построил БД, так что буду использовать связь один ко многим, хотя по факту она будет использоваться как один к одному
                            newEmployee.JobTitle = existingjobTitle;

                            context.Add(newEmployee);
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
