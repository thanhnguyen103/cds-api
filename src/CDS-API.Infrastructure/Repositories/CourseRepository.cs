using CDS_API.Domain.Entities;
using CDS_API.Domain.Interfaces.Repositories;
using CDS_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CDS_API.Infrastructure.Repositories;

public class CourseRepository(AppDbContext context) : ICourseRepository
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Retrieves all courses from the database, optionally filtered by course code and/or title.
    /// </summary>
    /// <param name="courseCode">The course code to filter by (optional).</param>
    /// <param name="courseTitle">The course title to filter by (optional).</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of courses.</returns>
    /// </summary>
    public Task<List<Course>> GetAllAsync(string? courseCode, string? courseTitle, CancellationToken cancellationToken)
    {
        return _context.Courses
            .Where(c =>
            (string.IsNullOrEmpty(courseCode) || EF.Functions.Like(c.Code, $"%{courseCode}%")) &&
            (string.IsNullOrEmpty(courseTitle) || EF.Functions.Like(c.Name, $"%{courseTitle}%")))
            .ToListAsync(cancellationToken);
    }
}