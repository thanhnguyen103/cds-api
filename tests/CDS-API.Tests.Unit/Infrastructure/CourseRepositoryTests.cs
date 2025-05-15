using Xunit;
using Microsoft.EntityFrameworkCore;
using CDS_API.Infrastructure.Data;
using CDS_API.Infrastructure.Repositories;
using CDS_API.Domain.Entities;
using System.Threading.Tasks;

public class CourseRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsCourses()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var context = new AppDbContext(options);
        context.Courses.Add(new Course { Id = 1, Name = "Test", Code = "TST" });
        context.SaveChanges();

        var repo = new CourseRepository(context);

        // Act
        var result = await repo.GetAllAsync(null, null, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal("Test", result.First().Name);
    }
}