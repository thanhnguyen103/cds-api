using Moq;
using CDS_API.Application.Services;
using CDS_API.Domain.Interfaces.Repositories;
using CDS_API.Domain.Entities;
using AutoMapper;
using CDS_API.Application.DTOs;
using CDS_API.Application.Mappings;

public class CourseServiceTests
{
    [Fact]
    public async Task GetCourses_ReturnsCourses_WhenRepositoryReturnsData()
    {
        // Arrange
        var mockRepo = new Mock<ICourseRepository>();
        mockRepo.Setup(r => r.GetAllAsync(null, null, CancellationToken.None))
            .ReturnsAsync(new List<Course> { new Course { Id = 1, Name = "Test", Code = "TST" } });
        
        var courseMapper = new MapperConfiguration(cfg => cfg.AddProfile(new CourseProfile())).CreateMapper();
       
        var service = new CourseService(mockRepo.Object, courseMapper);

        // Act
        var result = await service.GetCoursesAsync(new GetCoursesRequest(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test", result.ElementAt(0).CourseTitle);
    }
}