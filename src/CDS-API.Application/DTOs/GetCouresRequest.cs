namespace CDS_API.Application.DTOs;

public class GetCoursesRequest
{
    /// <summary>
    /// Optional. Filter by course title.
    /// </summary>
    public string? CourseTitle { get; set; }

    /// <summary>
    /// Optional. Filter by course code.
    /// </summary>
    public string? CourseCode { get; set; }
}