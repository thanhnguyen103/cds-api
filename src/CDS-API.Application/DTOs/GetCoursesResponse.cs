namespace CDS_API.Application.DTOs;

public class GetCoursesResponse
{
    /// <summary>
    /// Course title.
    /// </summary>
    public string CourseTitle { get; set; } = default!;

    /// <summary>
    /// Course code.
    /// </summary>
    public string CourseCode { get; set; } = default!;

    /// <summary>
    /// Effective date of the course.
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Expiry date of the course.
    /// </summary>
    public DateTime ExpiryDate { get; set; }
}