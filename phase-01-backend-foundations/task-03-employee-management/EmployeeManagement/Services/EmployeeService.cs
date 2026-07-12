using EmployeeManagement.Models;

namespace EmployeeManagement.Services;

public class EmployeeService
{
    private readonly List<Employee> _employees = new();

    public EmployeeService()
    {
        SeedData();
    }

    public void AddEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.FullName))
            throw new ArgumentException("Full name is required.");
        if (string.IsNullOrWhiteSpace(employee.Email))
            throw new ArgumentException("Email is required.");
        if (_employees.Any(e => e.EmployeeId == employee.EmployeeId))
            throw new InvalidOperationException("Employee ID already exists.");
        if (employee.Salary <= 0)
            throw new ArgumentException("Salary must be positive.");
        if (employee.HireDate.Date > DateTime.Today)
            throw new ArgumentException("Hire date cannot be in the future.");

        employee.IsActive = true;
        _employees.Add(employee);
    }

    public Employee? GetById(string id) => _employees.FirstOrDefault(e => e.EmployeeId == id);

    public void UpdateEmployee(string id, string? email, Department? dept, string? position, decimal? salary)
    {
        var emp = GetById(id) ?? throw new InvalidOperationException("Employee not found.");

        if (!string.IsNullOrWhiteSpace(email)) emp.Email = email;
        if (dept.HasValue) emp.Department = dept.Value;
        if (!string.IsNullOrWhiteSpace(position)) emp.Position = position;
        if (salary.HasValue)
        {
            if (salary <= 0) throw new ArgumentException("Salary must be positive.");
            emp.Salary = salary.Value;
        }
    }

    public void DeactivateEmployee(string id)
    {
        var emp = GetById(id) ?? throw new InvalidOperationException("Employee not found.");
        emp.IsActive = false;
    }

    public List<Employee> Search(string keyword)
    {
        return _employees.Where(e =>
            e.EmployeeId.Equals(keyword, StringComparison.OrdinalIgnoreCase) ||
            e.FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Employee> FilterByDepartment(string department, bool activeOnly = true)
    {
        return _employees.Where(e =>
            e.Department.ToString().Equals(department, StringComparison.OrdinalIgnoreCase) &&
            (!activeOnly || e.IsActive)).ToList();
    }

    public List<Employee> Sort(string option)
    {
        return option switch
        {
            "salary-asc" => _employees.OrderBy(e => e.Salary).ToList(),
            "salary-desc" => _employees.OrderByDescending(e => e.Salary).ToList(),
            "hire-asc" => _employees.OrderBy(e => e.HireDate).ToList(),
            "hire-desc" => _employees.OrderByDescending(e => e.HireDate).ToList(),
            "name" => _employees.OrderBy(e => e.FullName).ToList(),
            _ => _employees.ToList()
        };
    }

    public IReadOnlyList<Employee> GetAll() => _employees;

    private void SeedData()
    {
        _employees.AddRange(new[]
        {
            new Employee { EmployeeId = "EMP-001", FullName = "Mohamed Ayman", Email = "mohamed@test.com", Department = Department.IT, Position = "Backend Developer", Salary = 20000, HireDate = new DateTime(2025,1,10) },
            new Employee { EmployeeId = "EMP-002", FullName = "Sara Adel", Email = "sara@test.com", Department = Department.HR, Position = "HR Specialist", Salary = 12000, HireDate = new DateTime(2024,5,15) },
            new Employee { EmployeeId = "EMP-003", FullName = "Ahmed Tarek", Email = "ahmed@test.com", Department = Department.IT, Position = "Junior Developer", Salary = 9000, HireDate = new DateTime(2026,1,1) },
            new Employee { EmployeeId = "EMP-004", FullName = "Omar Samir", Email = "omar@test.com", Department = Department.Sales, Position = "Sales Executive", Salary = 11000, HireDate = new DateTime(2023,11,20) },
            new Employee { EmployeeId = "EMP-005", FullName = "Mariam Hassan", Email = "mariam@test.com", Department = Department.Finance, Position = "Accountant", Salary = 14000, HireDate = new DateTime(2022,9,11) },
            new Employee { EmployeeId = "EMP-006", FullName = "Khaled Ali", Email = "khaled@test.com", Department = Department.IT, Position = "DevOps Trainee", Salary = 10000, HireDate = new DateTime(2026,2,1) },
            new Employee { EmployeeId = "EMP-007", FullName = "Nour Emad", Email = "nour@test.com", Department = Department.Marketing, Position = "Content Specialist", Salary = 9500, HireDate = new DateTime(2025,7,8) },
            new Employee { EmployeeId = "EMP-008", FullName = "Youssef Nabil", Email = "youssef@test.com", Department = Department.Sales, Position = "Sales Manager", Salary = 18000, HireDate = new DateTime(2021,3,17), IsActive = false },
            new Employee { EmployeeId = "EMP-009", FullName = "Dina Farouk", Email = "dina@test.com", Department = Department.HR, Position = "Recruiter", Salary = 10500, HireDate = new DateTime(2024,2,13) },
            new Employee { EmployeeId = "EMP-010", FullName = "Hady Mahmoud", Email = "hady@test.com", Department = Department.IT, Position = "QA Engineer", Salary = 13000, HireDate = new DateTime(2025,10,1) },
            new Employee { EmployeeId = "EMP-011", FullName = "Salma Taha", Email = "salma@test.com", Department = Department.Finance, Position = "Finance Manager", Salary = 26000, HireDate = new DateTime(2020,12,12) },
            new Employee { EmployeeId = "EMP-012", FullName = "Ali Mostafa", Email = "ali@test.com", Department = Department.Support, Position = "Support Agent", Salary = 8000, HireDate = new DateTime(2026,3,5) }
        });
    }
}
