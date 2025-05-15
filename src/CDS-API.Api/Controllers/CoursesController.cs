using CDS_API.Application.DTOs;
using CDS_API.Application.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses([FromBody] GetCoursesRequest request, CancellationToken cancellationToken)
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