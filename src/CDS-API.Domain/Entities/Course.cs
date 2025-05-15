namespace CDS_API.Domain.Entities;

public class Course
{
    /// <summary>
    /// Unique identifier for the course.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Course title.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Course code.
    /// </summary>
    public string Code { get; set; } = default!;

    /// <summary>
    /// Effective date of the course.
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Expiry date of the course.
    /// </summary>
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Indicates if the course is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Course description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date and time when the course was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the course was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
