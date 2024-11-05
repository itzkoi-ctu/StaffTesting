using StaffServices.Model;

public interface IDepartmentRepository{
    IEnumerable<Department> GetDepartments();
    Department GetDepartment(int DepartmentId);
    
 }