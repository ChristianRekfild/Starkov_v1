using Starkov_v1.CustomErrors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starkov_v1.DataLoaders
{
    public class Loader
    {
        private string _dataPath;

        public Loader(string dataPath)
        {
            _dataPath = dataPath;
        }

        public void LoadData()
        {
            string[] files = Directory.GetFiles(_dataPath);

            List<string> resultFiles = new List<string>();

            // получаем файтические названия файлов
            foreach (string file in files) 
            { 
                int slashIndex = file.LastIndexOf('\\');
                resultFiles.Add(file.Substring(slashIndex +1));
            }

            // Проверяем, что все файлы есть на месте и всё хорошо
            bool jobTitleExists = resultFiles.Contains("jobtitle.tsv");
            bool departmentsExists = resultFiles.Contains("departments.tsv");
            bool employeesExists = resultFiles.Contains("employees.tsv");

            if ( !jobTitleExists || !departmentsExists || !employeesExists)
            {
                throw new RequiredFileNotFound("Не найден один или более нужных для импорта файлов!");
            }

            // JobTitle
            string jobTitlePath = _dataPath + "\\jobtitle.tsv";
            JobTitleLoader jobTitleLoader = new JobTitleLoader(jobTitlePath);
            jobTitleLoader.Load();

            

            // Departments
            // !!!! Мы его ещё запустим !!!
            string departmentsPath = _dataPath + "\\departments.tsv";
            DepartmentsLoader departmentsLoader = new DepartmentsLoader(departmentsPath);
            departmentsLoader.Load();


            // Employees
            string employeesPath = _dataPath + "\\employees.tsv";
            EmployeesLoader employeesLoader = new EmployeesLoader(employeesPath);
            employeesLoader.Load();

            // А теперь грузим подразделения ещё раз, чтобы найти менеджеров
            departmentsLoader.LoadAgainToSaveManager();

        }
    }
}
