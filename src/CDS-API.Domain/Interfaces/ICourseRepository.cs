using CDS_API.Domain.Entities; // Assuming Course entity is in this namespace

namespace CDS_API.Domain.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync(string? courseCode, string? courseTitle, CancellationToken cancellationToken);
}
