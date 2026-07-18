namespace StudentManagementApi.Models.DTOs;

public class PagedResultResponse
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<StudentResponseDto> Items { get; set; } = new List<StudentResponseDto>();
}
