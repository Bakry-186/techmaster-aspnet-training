using EmployeeManagement.Models;
using EmployeeManagement.Services;

namespace EmployeeManagement.UI;

public class ConsoleMenu
{
    private readonly EmployeeService _service = new();
    private readonly EmployeeReportService _reports;

    public ConsoleMenu()
    {
        _reports = new EmployeeReportService(_service);
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n====== Employee Management System ======");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Update Employee");
            Console.WriteLine("3. Deactivate Employee");
            Console.WriteLine("4. Search Employee");
            Console.WriteLine("5. Filter by Department");
            Console.WriteLine("6. Sort Employees");
            Console.WriteLine("7. Show Salary Reports");
            Console.WriteLine("8. View All Employees");
            Console.WriteLine("9. Exit");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid option.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1: Add(); break;
                    case 2: Update(); break;
                    case 3: Deactivate(); break;
                    case 4: Search(); break;
                    case 5: Filter(); break;
                    case 6: Sort(); break;
                    case 7: _reports.PrintSalaryReport(); break;
                    case 8: ViewAll(); break;
                    case 9: return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private void Add()
    {
        Console.Write("Employee ID: ");
        string id = Console.ReadLine() ?? string.Empty;
        Console.Write("Full name: ");
        string name = Console.ReadLine() ?? string.Empty;
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? string.Empty;
        Console.Write("Department: ");
        Enum.TryParse<Department>(Console.ReadLine(), true, out var dept);
        Console.Write("Position: ");
        string position = Console.ReadLine() ?? string.Empty;
        Console.Write("Salary: ");
        decimal.TryParse(Console.ReadLine(), out decimal salary);
        Console.Write("Hire date (yyyy-MM-dd): ");
        DateTime.TryParse(Console.ReadLine(), out DateTime hireDate);

        _service.AddEmployee(new Employee
        {
            EmployeeId = id,
            FullName = name,
            Email = email,
            Department = dept,
            Position = position,
            Salary = salary,
            HireDate = hireDate
        });

        Console.WriteLine("Employee added.");
    }

    private void Update()
    {
        Console.Write("Employee ID: ");
        string id = Console.ReadLine() ?? string.Empty;
        Console.Write("New email (blank to skip): ");
        string? email = Console.ReadLine();
        Console.Write("New department (blank to skip): ");
        string? deptInput = Console.ReadLine();
        Department? dept = string.IsNullOrWhiteSpace(deptInput) ? null : Enum.Parse<Department>(deptInput, true);
        Console.Write("New position (blank to skip): ");
        string? position = Console.ReadLine();
        Console.Write("New salary (blank to skip): ");
        string? salInput = Console.ReadLine();
        decimal? salary = string.IsNullOrWhiteSpace(salInput) ? null : decimal.Parse(salInput);

        _service.UpdateEmployee(id, string.IsNullOrWhiteSpace(email) ? null : email, dept,
            string.IsNullOrWhiteSpace(position) ? null : position, salary);
        Console.WriteLine("Employee updated.");
    }

    private void Deactivate()
    {
        Console.Write("Employee ID: ");
        _service.DeactivateEmployee(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Employee deactivated.");
    }

    private void Search()
    {
        Console.Write("Search by ID or name: ");
        var results = _service.Search(Console.ReadLine() ?? string.Empty);
        PrintEmployees(results);
    }

    private void Filter()
    {
        Console.Write("Department: ");
        var results = _service.FilterByDepartment(Console.ReadLine() ?? string.Empty);
        PrintEmployees(results);
    }

    private void Sort()
    {
        Console.WriteLine("1 salary-asc  2 salary-desc  3 hire-asc  4 hire-desc  5 name");
        Console.Write("Option: ");
        int.TryParse(Console.ReadLine(), out int opt);
        string key = opt switch { 1 => "salary-asc", 2 => "salary-desc", 3 => "hire-asc", 4 => "hire-desc", 5 => "name", _ => "name" };
        PrintEmployees(_service.Sort(key));
    }

    private void ViewAll() => PrintEmployees(_service.GetAll().ToList());

    private static void PrintEmployees(List<Employee> employees)
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        foreach (var e in employees)
        {
            Console.WriteLine($"{e.EmployeeId} | {e.FullName} | {e.Department} | {e.Salary:C} | {(e.IsActive ? "Active" : "Inactive")}");
        }
    }
}
