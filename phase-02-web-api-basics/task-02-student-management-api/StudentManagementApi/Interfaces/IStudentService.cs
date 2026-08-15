using StudentManagementApi.Models.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Interfaces;

public interface IStudentService
{
    Task<StudentResponseDto?> CreateStudent(CreateStudentDto createStudentDto);
    Task<StudentResponseDto?> GetStudentById(int id);
    Task<StudentResponseDto?> UpdateStudent(int id, UpdateStudentDto updateStudentDto);
    Task<bool> DeleteStudent(int id);
    Task<PagedResultResponse> GetAllStudents(string? search, string? trackName, bool? isActive, int pageNumber, int pageSize);
    Task<PagedResultResponse> GetStudentsByTrackName(string trackName, int pageNumber, int pageSize);
    Task<bool> UpdateStudentActivation(int id, UpdateStudentStatus updateStudentStatus);
    Task<StudentStatsDto> GetStudentStats();
}
