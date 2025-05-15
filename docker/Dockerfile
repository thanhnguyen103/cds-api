# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project files
COPY CDS-API.Api/*.csproj ./CDS-API.Api/
COPY CDS-API.Application/*.csproj ./CDS-API.Application/
COPY CDS-API.Domain/*.csproj ./CDS-API.Domain/

# Restore dependencies
RUN dotnet restore ./CDS-API.Api/CDS-API.Api.csproj

# Copy the rest of the application files
COPY CDS-API.Api/ ./CDS-API.Api/
COPY CDS-API.Application/ ./CDS-API.Application/
COPY CDS-API.Domain/ ./CDS-API.Domain/

# Build the application
RUN dotnet publish ./CDS-API.Api/CDS-API.Api.csproj -c Release -o /app/publish

# Use the official .NET runtime image as the runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/publish .

# Expose the port the application runs on
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "CDS-API.Api.dll"]