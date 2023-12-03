using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starkov_v1.CustomErrors
{
    internal class EmployeeNotFound : Exception
    {
        public EmployeeNotFound() { }

        public EmployeeNotFound(string message)
            : base(message) { }
    }
}
