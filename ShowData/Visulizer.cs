using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Starkov_v1.ShowData
{
    public class Visulizer
    {
        public void ShowData()
        {
            using (StarkovContext context = new StarkovContext())
            {
                var mainDepartments = context.Departments.Where(x => x.Parent == null);

                foreach (var dep in mainDepartments)
                {
                    ShowDepartment(dep, 0);

                }


            }
        }

        private void ShowDepartment(Department department, int indent) 
        {
            try
            {

            } catch (Exception ex)
            {
                Console.WriteLine("Что-то пошло не так опять...");
                Console.WriteLine($"Ошибка: {ex.ToString()}");
            }

            indent++;
            using (StarkovContext context = new StarkovContext())
            {
                string equals = GetSymbolString('=', indent);

                string star = GetSymbolString('*', indent);

                string minus = GetSymbolString('-', indent);


                Console.WriteLine($"{equals} Подразделение ID={department.Id} {department.Name}");

                Employee manager = context.Employees.SingleOrDefault(x => x.Departments.Contains(department));
                Console.WriteLine($"{star} Сотрудник ID=1 (должность ID=56)");

                var employees = context.Employees.Where(x => x.Departments.Contains(department));
                foreach (var employee in employees)
                {
                    Console.WriteLine($"{minus} Сотрудник ID={employee.Id} {employee.JobTitle}");
                }

                var innerDepartments = context.Departments.Where(x => x.Parent == department);
                foreach (var innerDepartment in innerDepartments)
                {
                    ShowDepartment(innerDepartment, indent);
                }
            }
        }

        /// <summary>
        /// возвращает строку из нужных символов. По сути - просто вставляет этот символ в строку нужное количество раз;
        /// </summary>
        /// <param name="c"></param>
        /// <returns>Строка их символа умноженная на количество</returns>
        private string GetSymbolString(char c, int count)
        {
            string result = string.Empty;

            for (int i = 0; i < count; i++)
            {
                result += c;
            }

            return result;
        }
    }
}
