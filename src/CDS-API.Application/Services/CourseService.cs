using CDS_API.Domain.Interfaces.Repositories;
using CDS_API.Application.DTOs;
using AutoMapper;

namespace CDS_API.Application.Services;

public interface ICourseService
{
    Task <IEnumerable<CourseDto>> GetCoursesAsync(GetCoursesRequest request, CancellationToken cancellationToken);
}

public class CourseService(ICourseRepository courseRepository, IMapper mapper) : ICourseService
{
    private readonly ICourseRepository _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<IEnumerable<CourseDto>> GetCoursesAsync(GetCoursesRequest request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetAllAsync(request.CourseCode, request.CourseTitle, cancellationToken);

        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }
}
