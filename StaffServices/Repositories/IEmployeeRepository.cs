


using StaffServices.Model;

public interface IEmployeeRepository{
    Task<IEnumerable<Employee>> GetEmployees();
    Task<Employee> GetEmployee(int id);
    Task<Employee> AddEmployee(Employee employee);
    Task<Employee> UpdateEmployee(Employee employee);

    Task<bool> DeleteEmployee(int id);
}