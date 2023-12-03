using System;
using System.Collections.Generic;

namespace Starkov_v1;

public partial class Department
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int? ManagerId { get; set; }

    public virtual ICollection<Department> InverseParent { get; set; } = new List<Department>();

    public virtual Employee? Manager { get; set; }

    public virtual Department? Parent { get; set; }
}
