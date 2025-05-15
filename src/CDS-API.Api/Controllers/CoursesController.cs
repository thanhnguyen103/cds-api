using Asp.Versioning;
using CDS_API.Application.DTOs;
using CDS_API.Application.Services;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Handles course-related operations.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    /// <summary>
    /// Retrieves a list of courses matching the given criteria.
    /// </summary>
    /// <param name="request">Filter criteria for courses.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of courses or an error message.</returns>
    /// <response code="200">Returns the list of courses.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If no courses are found.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetCoursesResponse>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCourses([FromQuery] GetCoursesRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Request body cannot be null" });
        }

        // Validate the request using FluentValidation
        var validator = new GetCoursesRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var courses = await _courseService.GetCoursesAsync(request, cancellationToken);
        if (courses == null || !courses.Any())
        {
            return NotFound(new { message = "No courses found matching the criteria." });
        }

        return Ok(courses);
    }
}