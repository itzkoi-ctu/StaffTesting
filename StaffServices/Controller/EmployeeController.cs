using Microsoft.AspNetCore.Mvc;
using StaffServices.Model;

[Route("api/employees")]
[ApiController]
public class EmployeeController : ControllerBase{
    private readonly IEmployeeRepository employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository){
        this.employeeRepository= employeeRepository;
    }


    [HttpGet]

    public async Task<ActionResult> GetEmployees(){
        try{
            return Ok(await employeeRepository.GetEmployees());
        }
        catch(Exception){
            return StatusCode(StatusCodes.Status500InternalServerError,"Error");
        }
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id){
        try{
            var result= await employeeRepository.GetEmployee( id);
            if(result == null){
                return NotFound();
            }
            return Ok(result);
        }catch(Exception){
            return StatusCode(StatusCodes.Status500InternalServerError,"Error");

        }
    }
    [HttpPost]
    public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
    {
        try{
            if(employee == null){
                return BadRequest();
            }
            var createdEmployee = await employeeRepository.AddEmployee(employee);
            return CreatedAtAction(nameof(GetEmployee), new {id = createdEmployee.EmployeeId});
        }catch(Exception){
            return StatusCode(StatusCodes.Status500InternalServerError,"Error.");
        }
    }
    
    
}