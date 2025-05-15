var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.CDS_API_Api>("api")
    .WithHttpsHealthCheck("/health");

builder.Build().Run();
