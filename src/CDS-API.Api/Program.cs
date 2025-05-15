using CDS_API.Application.Mappings;
using CDS_API.Application.Services;
using CDS_API.Domain.Interfaces.Repositories;
using CDS_API.Infrastructure.Repositories;
using CDS_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add controllers
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Dependency Injection for services
builder.Services.AddScoped<ICourseService, CourseService>();

// Dependency Injection for repositories
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

// Dependency Injection for AutoMapper
// builder.Services.AddAutoMapper(typeof(CourseProfile).Assembly);

// Dependency Injection for DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection for FluentValidation
// builder.Services.AddValidatorsFromAssemblyContaining<GetCoursesRequestValidator>();
builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapOpenApi();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
