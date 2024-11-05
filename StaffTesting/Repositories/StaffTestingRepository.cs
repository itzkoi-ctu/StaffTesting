


using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StaffServices.Model;

public class StaffTestingRepository
{
    private readonly Mock<IEmployeeRepository> mock;
    public static List<Employee> expectedList { get; set; }

    public StaffTestingRepository()
    {
        mock = new Mock<IEmployeeRepository>();

        // Khởi tạo danh sách mẫu expectedList
        expectedList = new List<Employee>
        {
            new Employee { EmployeeId = 1, FirstName = "Long", LastName = "Nguyen", DateOfBirth = new DateTime(2004, 10, 2), DepartmentId = 1, GenderId = 1, Email = "long.doe376@example.com" },
            new Employee { EmployeeId = 2, FirstName = "Dai", LastName = "Tran", DateOfBirth = new DateTime(2004, 10, 5), DepartmentId = 4, GenderId = 1, Email = "dai.doe376@example.com" },
            new Employee { EmployeeId = 3, FirstName = "Luc", LastName = "Tran", DateOfBirth = new DateTime(2004, 10, 5), DepartmentId = 4, GenderId = 2, Email = "luc.doe376@example.com" },
            new Employee { EmployeeId = 5, FirstName = "Bao", LastName = "Le", DateOfBirth = new DateTime(2004, 10, 5), DepartmentId = 4, GenderId = 1, Email = "bao.doe376@example.com" },

        };
    }
    //Ex1

    [Theory]
[InlineData(1)]
[InlineData(2)]
[InlineData(3)]
[InlineData(5)]

    //[Fact]
    
    public async Task GetStaffById_Test(int id)
    {
        // Tìm nhân viên mong đợi trong danh sách
        var expectedStaff = expectedList.FirstOrDefault(emp => emp.EmployeeId == id);
        
        // Giả lập phương thức GetEmployee với ID = 10
        mock.Setup(x => x.GetEmployee(id)).ReturnsAsync(expectedStaff);

        // Lấy đối tượng từ mock
        var result = await mock.Object.GetEmployee(id);

        // Kiểm tra kết quả trả về
        Assert.NotNull(result);
        Assert.Equal(expectedStaff.EmployeeId, result.EmployeeId);
        Assert.Equal(expectedStaff.FirstName, result.FirstName);
    }
    //EX2
    [Fact]
public void AddStaff_Test()
{
    // Tạo một đối tượng nhân viên mới
    var newStaff = new Employee 
    { 
        EmployeeId = 6, 
        FirstName = "Luc", 
        LastName = "Hoang", 
        DateOfBirth = new DateTime(2003, 7, 20), 
        DepartmentId = 3, 
        GenderId = 1, 
        Email = "luc.hoang@example.com" 
    };

    mock.Setup(x => x.AddEmployee(newStaff)).Verifiable();

    mock.Object.AddEmployee(newStaff);

    mock.Verify(x => x.AddEmployee(newStaff), Times.Once);
}
//EX3
[Fact]
public async Task AddStaff_NullStaff_Test()
{
    mock.Setup(x => x.AddEmployee(null)).Throws<ArgumentNullException>();

    await Assert.ThrowsAsync<ArgumentNullException>(() => mock.Object.AddEmployee(null));
}
//EX4

[Fact]
public async Task GetAllStaffs_WithData_Test()
{
    // Giả lập phương thức GetAllStaffs trả về danh sách có dữ liệu
    mock.Setup(x => x.GetEmployees()).ReturnsAsync(expectedList.AsEnumerable());

    // Gọi phương thức
    var result = await mock.Object.GetEmployees();

    // Kiểm tra kết quả trả về
    Assert.NotNull(result);
    Assert.Equal(expectedList.Count, result.Count());
}

[Fact]
public async void GetAllStaffs_EmptyList_Test()
{
    mock.Setup(x => x.GetEmployees()).ReturnsAsync(new List<Employee>().AsEnumerable());

    // Gọi phương thức
    var result = await mock.Object.GetEmployees();

    // Kiểm tra kết quả trả về
    Assert.NotNull(result);
    Assert.Empty(result);
}


//EX5
   
[Fact]
public async Task UpdateStaff_Success_Test()
{
    // Arrange
    var existingStaff = expectedList.First();
    existingStaff.FirstName = "Employee"; // Update the name

    mock.Setup(x => x.UpdateEmployee(existingStaff)).ReturnsAsync(existingStaff);

    // Act
    var result = await mock.Object.UpdateEmployee(existingStaff);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Employee", result.FirstName);
    mock.Verify(x => x.UpdateEmployee(existingStaff), Times.Once);
}

[Fact]
public async Task UpdateStaff_NonExistingStaff_Test()
{
    // Arrange
    var nonExistingStaff = new Employee 
    { 
        EmployeeId = 99, // Assuming this ID doesn't exist
        FirstName = "NonExistent", 
        LastName = "Employee" 
    };

    mock.Setup(x => x.UpdateEmployee(nonExistingStaff)).Throws<InvalidOperationException>();

    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(() => mock.Object.UpdateEmployee(nonExistingStaff));
}

[Fact]
public async Task UpdateStaff_NullStaff_Test()
{
    // Arrange
    mock.Setup(x => x.UpdateEmployee(null)).Throws<ArgumentNullException>();

    // Act & Assert
    await Assert.ThrowsAsync<ArgumentNullException>(() => mock.Object.UpdateEmployee(null));
}
// EX7
[Fact]
public async Task DeleteStaff_Success_Test()
{
    // Arrange
    var existingStaffId = 1; // Assuming this ID exists
    // mock.Setup(x => x.DeleteEmployee(existingStaffId)).ReturnsAsync(true);
    mock.Setup(x => x.DeleteEmployee(existingStaffId)).ReturnsAsync(true);

    // Act
    var result = await mock.Object.DeleteEmployee(existingStaffId);

    // Assert
    Assert.True(result);
    mock.Verify(x => x.DeleteEmployee(existingStaffId), Times.Once);
}

[Fact]
public async Task DeleteStaff_NonExistingStaff_Test()
{
    // Arrange
    var nonExistingStaffId = 99; // Assuming this ID doesn't exist
    mock.Setup(x => x.DeleteEmployee(nonExistingStaffId)).ReturnsAsync(false);

    // Act
    var result = await mock.Object.DeleteEmployee(nonExistingStaffId);

    // Assert
    Assert.False(result);
}

[Fact]
public async Task DeleteStaff_InvalidId_Test()
{
    // Arrange
    var invalidStaffId = -1; // Assuming negative ID is invalid
    mock.Setup(x => x.DeleteEmployee(invalidStaffId)).ThrowsAsync(new ArgumentOutOfRangeException());

    // Act & Assert
    await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => mock.Object.DeleteEmployee(invalidStaffId));
}


}
