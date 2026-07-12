using EmployeeManagement.Models;
using EmployeeManagement.Services;

namespace EmployeeManagement.Services;

public class EmployeeReportService
{
    private readonly EmployeeService _employeeService;

    public EmployeeReportService(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public decimal GetAverageSalary()
    {
        var active = _employeeService.GetAll().Where(e => e.IsActive).ToList();
        return active.Count == 0 ? 0 : active.Average(e => e.Salary);
    }

    public Employee? GetHighestPaid()
        => _employeeService.GetAll().OrderByDescending(e => e.Salary).FirstOrDefault();

    public Employee? GetLowestPaid()
        => _employeeService.GetAll().OrderBy(e => e.Salary).FirstOrDefault();

    public decimal GetTotalPayroll()
        => _employeeService.GetAll().Where(e => e.IsActive).Sum(e => e.Salary);

    public Dictionary<Department, int> GetCountByDepartment()
    {
        return _employeeService.GetAll()
            .Where(e => e.IsActive)
            .GroupBy(e => e.Department)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public (int active, int inactive) GetActiveInactiveCounts()
    {
        var all = _employeeService.GetAll();
        return (all.Count(e => e.IsActive), all.Count(e => !e.IsActive));
    }

    public void PrintSalaryReport()
    {
        var highest = GetHighestPaid();
        var lowest = GetLowestPaid();
        var (active, inactive) = GetActiveInactiveCounts();

        Console.WriteLine($"Average salary: {GetAverageSalary():C}");
        Console.WriteLine($"Highest: {highest?.FullName} ({highest?.Salary:C})");
        Console.WriteLine($"Lowest: {lowest?.FullName} ({lowest?.Salary:C})");
        Console.WriteLine($"Total payroll (active): {GetTotalPayroll():C}");
        Console.WriteLine($"Active: {active} | Inactive: {inactive}");

        Console.WriteLine("Count by department:");
        foreach (var item in GetCountByDepartment())
            Console.WriteLine($"  {item.Key}: {item.Value}");
    }
}
