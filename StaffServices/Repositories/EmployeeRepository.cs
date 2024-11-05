


using Microsoft.EntityFrameworkCore;
using StaffServices.Model;

public class EmployeeRopository : IEmployeeRepository
{
    public Staffs3Context staffs3Context;

    public EmployeeRopository(Staffs3Context staffs3Context){
        this.staffs3Context=staffs3Context;
    }
    public async Task<Employee> AddEmployee(Employee employee)
    {
        if(employee == null){
            throw new ArgumentNullException(nameof(employee), "Employee cannot be null");

        }
        var result = await staffs3Context.Employees.AddAsync(employee);
        await staffs3Context.SaveChangesAsync();

        return result.Entity;
    }
    

    public async  Task<bool> DeleteEmployee(int id)
    {
        var result= await staffs3Context.Employees.FirstOrDefaultAsync(e => e.EmployeeId== id);
        if(result != null){
            staffs3Context.Employees.Remove(result);
            await staffs3Context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Employee> GetEmployee(int id)
    {
     return await staffs3Context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.EmployeeId== id);

    }

    public async Task<IEnumerable<Employee>> GetEmployees()
    {
        return await staffs3Context.Employees.ToListAsync();
    }

    public async Task<Employee> UpdateEmployee(Employee employee)
    {
        var result= await staffs3Context.Employees.FirstOrDefaultAsync(e => e.EmployeeId== employee.EmployeeId);
        if (result != null){
            result.FirstName= employee.FirstName;
            result.DateOfBirth= employee.DateOfBirth;
            result.Email=employee.Email;
            result.LastName=employee.LastName;
            result.DepartmentId=employee.DepartmentId;
            result.Gender=employee.Gender;
            await staffs3Context.SaveChangesAsync();
            return result;

        }
        return null;
    }
}