
using System;

using StaffServices.Model;

public class DepartmentRepository: IDepartmentRepository{
    public Staffs3Context staffs3Context;
    public DepartmentRepository(Staffs3Context staffs3Context){
        this.staffs3Context= staffs3Context;
    }

    

    public Department GetDepartment(int DepartmentId)
    {
       return staffs3Context.Departments.FirstOrDefault(d => d.DepartmentId== DepartmentId);
    }

    public IEnumerable<Department> GetDepartments()
    {
        return staffs3Context.Departments;
    }
}