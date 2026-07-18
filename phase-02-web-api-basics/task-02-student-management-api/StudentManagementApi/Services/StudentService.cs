using StudentManagementApi.Interfaces;
using StudentManagementApi.Models;
using StudentManagementApi.Models.DTOs;

namespace StudentManagementApi.Services;

public class StudentService : IStudentService
{
    private readonly List<Student> _students = new();
    private int _nextId = 1;

    public StudentService()
    {
        SeedStudents();
    }

    public Task<PagedResultResponse> GetAllStudents(string? search, string? trackName, bool? isActive, int pageNumber, int pageSize)
    {
        var query = ApplyFilters(_students, search, trackName, isActive);
        return Task.FromResult(ToPagedResult(query, pageNumber, pageSize));
    }

    public Task<PagedResultResponse> GetStudentsByTrackName(string trackName, int pageNumber, int pageSize)
    {
        var query = _students.Where(s => s.TrackName.Equals(trackName, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(ToPagedResult(query, pageNumber, pageSize));
    }

    public Task<bool> UpdateStudentActivation(int id, UpdateStudentStatus updateStudentStatus)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return Task.FromResult(false);
        }

        student.IsActive = updateStudentStatus.IsActive;
        return Task.FromResult(true);
    }

    public Task<StudentStatsDto> GetStudentStats()
    {
        return Task.FromResult(new StudentStatsDto
        {
            TotalStudents = _students.Count,
            TotalActiveStudents = _students.Count(s => s.IsActive),
            TotalInactiveStudents = _students.Count(s => !s.IsActive),
            StudentsByTrack = _students
                .GroupBy(s => s.TrackName)
                .ToDictionary(g => g.Key, g => g.Count())
        });
    }

    public Task<Student?> GetStudentById(int id)
    {
        return Task.FromResult(_students.FirstOrDefault(s => s.Id == id));
    }

    public Task<Student?> CreateStudent(CreateStudentDto createStudentDto)
    {
        if (EmailExists(createStudentDto.Email))
        {
            return Task.FromResult<Student?>(null);
        }

        var student = new Student
        {
            Id = _nextId++,
            Name = createStudentDto.Name,
            Email = createStudentDto.Email,
            Phone = createStudentDto.Phone,
            TrackName = createStudentDto.TrackName,
            GithubUrl = createStudentDto.GithubUrl,
            LinkedInUrl = createStudentDto.LinkedInUrl,
            EnrollmentDate = DateTime.UtcNow,
            IsActive = true
        };

        _students.Add(student);
        return Task.FromResult<Student?>(student);
    }

    public Task<Student?> UpdateStudent(int id, UpdateStudentDto updateStudentDto)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return Task.FromResult<Student?>(null);
        }

        if (EmailExists(updateStudentDto.Email, id))
        {
            return Task.FromResult<Student?>(null);
        }

        student.Name = updateStudentDto.Name;
        student.Email = updateStudentDto.Email;
        student.Phone = updateStudentDto.Phone;
        student.TrackName = updateStudentDto.TrackName;
        return Task.FromResult<Student?>(student);
    }

    public Task<bool> DeleteStudent(int id)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return Task.FromResult(false);
        }

        _students.Remove(student);
        return Task.FromResult(true);
    }

    private static IEnumerable<Student> ApplyFilters(IEnumerable<Student> students, string? search, string? trackName, bool? isActive)
    {
        var query = students.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s =>
                s.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(trackName))
        {
            query = query.Where(s => s.TrackName.Equals(trackName, StringComparison.OrdinalIgnoreCase));
        }

        if (isActive.HasValue)
        {
            query = query.Where(s => s.IsActive == isActive.Value);
        }

        return query;
    }

    private static PagedResultResponse ToPagedResult(IEnumerable<Student> query, int pageNumber, int pageSize)
    {
        var filtered = query.ToList();
        var items = filtered
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToResponse)
            .ToList();

        return new PagedResultResponse
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = filtered.Count,
            TotalPages = filtered.Count == 0 ? 0 : (int)Math.Ceiling((double)filtered.Count / pageSize),
            Items = items
        };
    }

    private bool EmailExists(string email, int? excludeId = null)
    {
        return _students.Any(s =>
            s.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            (!excludeId.HasValue || s.Id != excludeId.Value));
    }

    private void SeedStudents()
    {
        _students.AddRange(new[]
        {
            new Student { Id = _nextId++, Name = "Abdelrahman Abdelhamid", Email = "abdelrahman@techmaster.dev", Phone = "01000000001", TrackName = "ASP.NET Backend", GithubUrl = "https://github.com/abdelrahman", IsActive = true, EnrollmentDate = DateTime.UtcNow.AddMonths(-2) },
            new Student { Id = _nextId++, Name = "Sara Hassan", Email = "sara@techmaster.dev", Phone = "01000000002", TrackName = "ASP.NET Backend", IsActive = true, EnrollmentDate = DateTime.UtcNow.AddMonths(-1) },
            new Student { Id = _nextId++, Name = "Omar Ali", Email = "omar@techmaster.dev", Phone = "01000000003", TrackName = "Frontend", IsActive = true, EnrollmentDate = DateTime.UtcNow.AddDays(-20) },
            new Student { Id = _nextId++, Name = "Mona Farouk", Email = "mona@techmaster.dev", Phone = "01000000004", TrackName = "Frontend", IsActive = false, EnrollmentDate = DateTime.UtcNow.AddDays(-45) },
            new Student { Id = _nextId++, Name = "Youssef Nabil", Email = "youssef@techmaster.dev", Phone = "01000000005", TrackName = "DevOps", IsActive = true, EnrollmentDate = DateTime.UtcNow.AddDays(-10) }
        });
    }

    private static StudentResponseDto MapToResponse(Student student)
    {
        return new StudentResponseDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Phone = student.Phone,
            TrackName = student.TrackName,
            EnrollmentDate = student.EnrollmentDate,
            IsActive = student.IsActive,
            GithubUrl = student.GithubUrl,
            LinkedInUrl = student.LinkedInUrl
        };
    }
}
