using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starkov_v1.CustomErrors
{
    internal class DepartmentNotFound : Exception
    {
        public DepartmentNotFound() { }

        public DepartmentNotFound(string message)
            : base(message) { }
    }
}
