using System;
using System.Collections.Generic;

namespace Starkov_v1;

public partial class Employee
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? JobTitleId { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual JobTitle? JobTitle { get; set; }
}
