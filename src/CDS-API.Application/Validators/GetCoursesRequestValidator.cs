using FluentValidation;
using CDS_API.Application.DTOs;

public class GetCoursesRequestValidator : AbstractValidator<GetCoursesRequest>
{
    public GetCoursesRequestValidator()
    {
        // At least one conditional field must be provided
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.CourseTitle) || !string.IsNullOrWhiteSpace(x.CourseCode))
            .WithMessage("Missing conditional field, must provide at least 1");

        // Example: Add more sophisticated rules here
        // RuleFor(x => x.CourseCode)
        //     .Matches(@"^[A-Z]{2,5}\d{3}$")
        //     .When(x => !string.IsNullOrWhiteSpace(x.CourseCode))
        //     .WithMessage("CourseCode must match pattern (e.g., CS101)");
    }
}