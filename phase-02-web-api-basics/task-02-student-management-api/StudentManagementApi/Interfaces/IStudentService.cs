using StudentManagementApi.Models.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Interfaces;

public interface IStudentService
{
    Task<Student?> CreateStudent(CreateStudentDto createStudentDto);
    Task<Student?> GetStudentById(int id);
    Task<Student?> UpdateStudent(int id, UpdateStudentDto updateStudentDto);
    Task<bool> DeleteStudent(int id);
    Task<PagedResultResponse> GetAllStudents(string? search, string? trackName, bool? isActive, int pageNumber, int pageSize);
    Task<PagedResultResponse> GetStudentsByTrackName(string trackName, int pageNumber, int pageSize);
    Task<bool> UpdateStudentActivation(int id, UpdateStudentStatus updateStudentStatus);
    Task<StudentStatsDto> GetStudentStats();
}
