using System;
using System.Collections.Generic;

namespace StaffServices.Model;

public partial class Gender
{
    public int GenderId { get; set; }

    public string GenderDescription { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
